using System.Diagnostics;
using System.Net;
using bookingSite.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace bookingSite.Controllers
{ 
    public class FrontController : Controller
    {
        //properties
        private readonly ILogger<FrontController> _logger;
        private static int reqCount=0;

        //constructors
        public FrontController(ILogger<FrontController> logger)
        {
            _logger = logger;

        }

        //methods
        [HttpGet("/")]
        public IActionResult index()
        {
            IPAddress remoteIpAddress = HttpContext.Connection.RemoteIpAddress; //::1 is the loopback address in IPv6.Think of it as the IPv6 version of 127.0. 0.1.
            DateTime ts = DateTime.Now;
            reqCount++;
            _logger.LogInformation("{0}    Remote Host {1}    Request No. {2}", ts, remoteIpAddress.ToString(),reqCount.ToString());

            return View("/Views/Home/test.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}