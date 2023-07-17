function searchHotels()
{
    const country = document.querySelector("#countryInp").value;
    const checkInDate = document.querySelector("#dateFromInp").value;
    const checkOutDate = document.querySelector("#dateToInp").value;
    
    const url = `/searchHotelsInc?country=${country}&checkInDate=${checkInDate}&checkOutDate=${checkOutDate}&starCount=${starCount}`;

    //fetch
    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.getElementById("_hotelsViewDiv").innerHTML = data;
        });
}

//function searchHotels() {
 
//    const url = "/searchHotelsInc";
    
//    ////jquery
//    //const $j = jQuery.noConflict();

//    //$j.ajax({
//    //    url: url,
//    //    type: "GET",
//    //    success: function (data) {
//    //        $j("#_hotelsViewDiv").html(data);
//    //    }
//    //});     

//    ////xml
//    //let xhttp = new XMLHttpRequest();
//    //xhttp.onreadystatechange = function () {
//    //    if (this.status == 200) {
//    //        document.getElementById("_hotelsViewDiv").innerHTML = this.responseText;
//    //    }
//    //}
//    //xhttp.open("GET", url, true);
//    //xhttp.send();

//    ////fetch
//    fetch(url)
//        .then(response => response.text())
//        .then(data => {
//            document.getElementById("_hotelsViewDiv").innerHTML = data;
//        }
//    );
//} 

function searchRoomsByHotel(middleLayerLevel)
{
    let currentHotelId = 0;
    if (middleLayerLevel == 3) return;
    if (middleLayerLevel == 1 && $(".bottomLayer") != null) return;
    if (middleLayerLevel == 1 && $(".bottomLayer")==null) { currentHotelId = $(".belowLayer>.middle").id; }
    if (middleLayerLevel == 2) { currentHotelId = $(".middleLayer>.middle").id; }
 
    const checkInDate = document.querySelector("#dateFromInp").value;
    const checkOutDate = document.querySelector("#dateToInp").value;
    const url = `/searchRoomsByHotel?currentHotelId=${currentHotelId}&cIn=${checkInDate}&cOut=${checkOutDate}`;

    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.getElementById("_roomsViewDiv").innerHTML = data;

                if (middleLayerLevel == 2) {
                    const roomLayer = $(".bottomLayer");
                    roomLayer.classList.remove("bottomLayer"); roomLayer.classList.add("belowLayer");
                }
        });
}

swipeDiv.on("swipeleft", (ev) => {

    if (middleLayerLevel == 2) searchRoomsByHotel(middleLayerLevel);
});

swipeDiv.on("swiperight", (ev) => {

    if (middleLayerLevel == 2) searchRoomsByHotel(middleLayerLevel);
});
swipeDiv.on("swipedown", (ev) => {

    if (middleLayerLevel == 1) searchRoomsByHotel(middleLayerLevel);
});


let card; 
let roomContent; 
function startBooking()
{
    card = $(".middleLayer>.middle>.content");
    roomContent = card.innerHTML;

    const hotelId = $(".aboveLayer>.middle").id;
    const roomId = $(".middleLayer>.middle").id;
    const checkInDate =new Date( $("#dateFromInp").value );
    const checkOutDate =new Date( $("#dateToInp").value);
    const dayCount = dateDiffInDays(checkInDate, checkOutDate);
    let price = dayCount * Number($("#pricePerNight").innerText);
    if (isNaN(price)) price = "(choose timeframe first)";
    let bookingContent = `
    <h3>Booking</h3>
    <h4>Costs: ${price}<span> $</span></h4>
    <form id="bookForm" method="post" action="/doBookCreation">
        <input type="hidden" name="hotelId" value=${hotelId} />
        <input type="hidden" name="roomId" value=${roomId}>
        <input type="hidden" name="checkInDate" value=${$("#dateFromInp").value}>
        <input type="hidden" name="checkOutDate" value=${$("#dateToInp").value}>
        <input type="hidden" name="price" value=${price}>
        <input type="hidden" name="hotelName" value=${$("#hotelName").innerText}>
        <input type="text" name="name" placeholder="Name" required>
        <input type="text" name="phone" placeholder="Phone" required>
        <input type="text" name="email" placeholder="Email" required>
        <br>
        <button class="myButton" type="submit">Book</button><br>
        <button class="myButton" type="button" onclick="cancelBooking()">Cancel</button>
    </form>`;
    
    card.innerHTML = "";
    card.innerHTML = bookingContent;
}

function cancelBooking()
{
    card.innerHTML = "";
    card.innerHTML = roomContent;
}

function dateDiffInDays(a, b) {
    const _MS_PER_DAY = 1000 * 60 * 60 * 24;
    // Discard the time and time-zone information.
    const utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    const utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc2 - utc1) / _MS_PER_DAY);
}

function getBookingsByEmail(p_email=null)
{
    let url;
    if(p_email==null){
        url = `/getBookingsByEmail?email=${$("#emailInp").value}`;
    }
    else url = `/getBookingsByEmail?email=${p_email}`;
   
    document.cookie = `email=${$("#emailInp").value}`;//store email in cookie
    //HttpContext.Current.Session["email"] = "emailblabla";

    fetch(url)
        .then(response => response.text())
        .then(data => {
            $("#loginCard>.content").innerHTML = data
        });
}

function deleteBooking(id) {

    const url = `/deleteBooking?bookingId=${id}`;

    fetch(url)
        .then(response => response.text())
        .then(() => { 
            const email = document.cookie.split('=')[1];//get email from cookie
            getBookingsByEmail(email);
        });
}

function getStats(id)
{
    const to = document.querySelector("#dateToInp").value;
    const from = document.querySelector("#dateFromInp").value;
        
    const url = `/getStats?id=${id}&from=${from}&to=${to}`;

    fetch(url)
        .then(response => response.text())
        .then(data => {
            document.querySelector("#diagramDiv").innerHTML = data

        });
}
