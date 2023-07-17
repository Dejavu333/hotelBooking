using System.Collections.Generic;

namespace bookingSite.Models
{
	public class Guest
	{
		//properties
		public int ID;
		public string name;
		public string email;
		public List<Booking> bookings;

		//constructors
		public Guest()
		{
			bookings = new List<Booking>();


		}

		//methods

	}

	public class Booking
	{
		//!fproperties
		public dynamic _id { get; set; }

		public DateTime checkInDate { get; set; }
		public DateTime checkOutDate { get; set; }
		public double price { get; set; }
		public string[] guestNames { get; set; }
		public string phoneNum { get; set; }
		public string email { get; set; }
        public string hotelName { get; set; }
        //!constructors
        public Booking(DateTime p_checkInDate, DateTime p_checkOutDate, double p_price, string p_phoneNum, string p_email, string[] p_guestNames, string p_hotelName)
		{
            hotelName = p_hotelName;
            checkInDate = p_checkInDate;
			checkOutDate = p_checkOutDate;
			price = p_price;
			phoneNum = p_phoneNum;
			email = p_email;
			guestNames = p_guestNames;
		}//Support for DateTime and TimeOnly will be/was added in 13.0.2 which is currently in Beta 2. In earlier versions applications should use custom JSON converters.
		//!methods
	}

	public class Room
	{
		//!properties
		public dynamic _id {get;set;}
		public List<Booking>? _bookings { get; set; }

		public string _name { get; set; }
		public int _capacity { get; set; }
		public int _pricePerNight { get; set; }
		public string _description { get; set; }
        public string _imgPath { get; set; }
        //!constructors
        public Room(string p_name, int p_capacity, int p_pricePerNight, string p_description, string p_imgPath, List<Booking>p_bookings=null)
		{
			_name = p_name;
			_capacity = p_capacity;
			_pricePerNight = p_pricePerNight;
			_description = p_description;
            _imgPath = p_imgPath;
            _bookings = p_bookings;
		}
		//!methods

	}

	public class Hotel //todo rooms-ra a roomIds-t aztan aztan törölni v updatelni a hoteleket mongoban
	{
		//!properties
		public dynamic _id { get; set; } //set-->init
		public List<Room>? _rooms { get; set; } //private set

		public string _name { get; set; }
		public int _starCount { get; set; }
		public string _country { get; set; }
		public string _city { get; set; }
		public string _address { get; set; }
		public int _startingPrice { get; set; }
		public string _description { get; set; }
		public string _imgPath { get; set; }

		//!constructors
		public Hotel(string p_name, string p_country, string p_city,
					 string p_address, int p_starCount, string p_description,
					 string p_imgPath, List<Room> p_rooms=null)
		{
			this._name = p_name;
			this._starCount = p_starCount;
			this._country = p_country;
			this._city = p_city;
			this._address = p_address;
			this._description = p_description;
			this._imgPath = p_imgPath;
			this._rooms = p_rooms;

			this._startingPrice = calculateStartingPrice();
		}      
        //!methods
        private int calculateStartingPrice()
        {
			if (_rooms == null) return -1;
            int minPrice = int.MaxValue;
            foreach (Room room in _rooms)
            {
                if (room._pricePerNight < minPrice)
                {
                    minPrice = room._pricePerNight;
                }
            }
			return _startingPrice = minPrice;
        }

    }
	public enum SEASON { SPRING, SUMMER, AUTUMN, WINTER };
	public enum STATUS { FREE, BOOKED, UNDERRENOVATION };
}
