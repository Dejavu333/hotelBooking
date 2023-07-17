using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Reflection.Emit;
using bookingSite.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace bookingSite.Controllers
{
    public class HotelController : Controller
	{
		//!properties
		private readonly ILogger<HotelController> _logger;
		private readonly AbstractHotelRepository _repository;
        private IHostingEnvironment Environment;

        //!constructors
        public HotelController(ILogger<HotelController> logger,
							   AbstractHotelRepository repository, IHostingEnvironment env)
		{
			_logger = logger;
            _repository = repository;
			Environment = env;
        }

        //!methods
        //ajax
        [HttpGet("/searchHotelsInc")]
		public IActionResult searchHotelsInc(string country, string checkInDate, string checkOutDate, string starCount)
		{
			//var url = Request.GetDisplayUrl();
			////var query = HttpUtility.ParseQueryString(url.Query);
			////var var2 = query.Get("var2");

			string[] details = { country, checkInDate, checkOutDate, starCount };
			var hotels = _repository.readByDetails(details);
			
			this.ViewData["hotelsSearched"] = hotels; //todo valtozhatna a neve

			return PartialView("/Views/Home/_hotelsView.cshtml");
		}
     
        //ajax
        [HttpGet("/searchRoomsByHotel")]
        public IActionResult searchRoomsByHotel(string currentHotelId, string cIn = null, string cOut = null)
        {
            var hotel = _repository.Read(currentHotelId) as Hotel;

            List<Room> availableRooms=new List<Room>();
            //if a room contains booking with bad timeframe leave it out
            if (cOut != null && cOut != null) { 
                DateTime cin = DateTime.Parse(cIn);
                DateTime cout = DateTime.Parse(cOut);
                foreach (Room r in hotel._rooms)
                {
                    if (r._bookings==null || r._bookings.All(b => cin > b.checkOutDate || cout < b.checkInDate))
                    {
                        availableRooms.Add(r);
                    }
                }
                //availableRooms = hotel._rooms.FindAll(r => r._bookings.All(b => cin > b.checkOutDate || cout < b.checkInDate)) as List<Room>;
            }
            else availableRooms = hotel._rooms;

            this.ViewData["roomsSearched"] = availableRooms;

            return PartialView("/Views/Home/_roomsView.cshtml");
		}


		[HttpGet("/serveHotelCreation")]
		public IActionResult serveHotelCreation()
		{
			var hotels = _repository.ReadEvery();
            this.ViewData["everyHotel"] = hotels;
			return View("/Views/Home/hotelCreation.cshtml");	
		}

        
        [HttpGet("/getStats")]
        public IActionResult getStats(string id, string from, string to)
        { 
            var hotel = _repository.Read(new ObjectId(id));
            this.ViewData["chosenHotel"] = hotel;
			this.ViewData["from"] = from;
			this.ViewData["to"] = to;
			return PartialView("/Views/Home/_diagram.cshtml");
        }

        
        [HttpGet("/getBookingsByEmail")]
		public IActionResult getBookingsByEmail(string email)
		{
            List<Booking> bookings = new List<Booking>();
            bookings = _repository.readBookingsByEamil(email);

            this.ViewData["bookings"] = bookings;
			return View("/Views/Home/_loginCard.cshtml");
        }

        
        [HttpGet("/deleteBooking")]
        public IActionResult deleteBooking(string bookingId)
        {
			var hotels = _repository.ReadEvery() as List<Hotel>;
			//delete booking from hotels then update
			Hotel hotel=null;
            foreach (Hotel h in hotels)
            {
                foreach (Room r in h._rooms)
                {
                    foreach (Booking b in r._bookings)
                    {
                        if (b._id == new ObjectId(bookingId))
                        {
                            r._bookings.Remove(b);
                            hotel = h;
                            goto prisonBreak;
                        }
                    }
                }
            }
            prisonBreak:
            if (hotel != null) _repository.Update(hotel);

            return View("/Views/Home/_loginCard.cshtml");
        }
    

        [HttpPost("/doHotelCreation")]
		public IActionResult doHotelCreation()
		{
            //?process post data for hotel
            string hotelName = Request.Form["hotelName"];
            string country = Request.Form["country"];
            string city = Request.Form["city"];
            string address = Request.Form["address"];
            int starCount = Convert.ToInt32( Request.Form["starCount"] );
			string hotelDescription = Request.Form["hotelDescription"];
            IFormFileCollection files = Request.Form.Files;
			string hotelImgPath="";
            List<Room> rooms = new List<Room>();

            List<string> roomImgPaths = new List<string>();
            foreach (var file in files)
            {
                //img name
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim(' ', '"');

                //dest
                string wwwPath = this.Environment.WebRootPath;
                string contentPath = this.Environment.ContentRootPath;

				//choose adequate path
				string path = "";
                if (file.Name == "hotelImage")
				{
					path = Path.Combine(this.Environment.WebRootPath, "imgs/hotelsImgs");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					hotelImgPath = fileName;
				}
                else if (file.Name == "roomImage")
                {
                    path = Path.Combine(this.Environment.WebRootPath, "imgs/roomsImgs");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
					roomImgPaths.Add(fileName);
                }


                //write to disk
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                    ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                }
            }

			for (int i = 0; i < roomImgPaths.Count; i++)
			{
				int c = i + 1;
                //?process post data for rooms
                string roomName = Request.Form[$"roomName{c}"];
                int capacity = Convert.ToInt32(Request.Form[$"capacity{c}"]);
                int pricePerNight = Convert.ToInt32(Request.Form[$"pricePerNight{c}"]);
                string roomDescription = Request.Form[$"roomDescription{c}"];
                string roomImgPath = roomImgPaths[i];
  
                Room room = new Room(roomName, capacity, pricePerNight, roomDescription, roomImgPath);
                room._id = ObjectId.GenerateNewId();
                
                rooms.Add(room);
            }

            //create hotel
            _repository.Create(new Hotel(hotelName, country, city, address, starCount, hotelDescription, hotelImgPath, rooms));

            //redirect
            return RedirectToAction("serveHotelCreation");
        }


        [HttpPost("/doBookCreation")]
        public IActionResult doBookingCreation()
        {
            //?process post data for booking
            string hotelName = Request.Form["hotelName"];
            string hotelId = Request.Form["hotelId"];
            string roomId = Request.Form["roomId"];
            DateTime checkInDate = DateTime.Parse(Request.Form["checkInDate"]);
            DateTime checkOutDate = DateTime.Parse(Request.Form["checkOutDate"]);
            string name = Request.Form["name"];
            string phone = Request.Form["phone"];
            string email = Request.Form["email"];
            int price = Convert.ToInt32(Request.Form["price"]);

            Booking b = new Booking(checkInDate, checkOutDate, price, phone, email, new string[] { name }, hotelName);
            b._id = ObjectId.GenerateNewId();

            Hotel hotel = _repository.Read(hotelId) as Hotel;
            Room r = hotel._rooms.Find(r => r._id == new ObjectId(roomId));
            (r._bookings ??= new List<Booking>()).Add(b);

            _repository.Update(hotel);
            
            sendMail(email,
                    $"Booking Confirmation {b._id}",
                    $"Your booking has been confirmed!\nArrival:{Request.Form["checkInDate"]}\nLeaving:{Request.Form["checkInDate"]}\n" +
                    $"Total Price: {price} $\n" +
                    $"Have a nice holiday!\n" +
                    $"Best wishes Team D-Served");

            return View("/Views/Home/successBooking.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

        //?SERVICE
		void sendMail(string p_to, string p_subject,string p_msg) 
        { 
            MailAddress to = new MailAddress(p_to);
            MailAddress from = new MailAddress("tosozteam@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = p_subject;
            message.Body = p_msg;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587) //Gmail 
            {
                Credentials = new NetworkCredential("tosozteam", "gwscoskejevrwytb"),
                EnableSsl = true
            };
            // code in brackets above needed if authentication required
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
	}//controller ends
}//namespace ends