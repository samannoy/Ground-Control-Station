using Microsoft.AspNetCore.Mvc;
using RouteMapApplication.Models;
using System.Diagnostics;

namespace RouteMapApplication.Controllers
{
    public class RouteMapController : Controller
    {
        private readonly ILogger<RouteMapController> _logger;

        public RouteMapController(ILogger<RouteMapController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetCoordinates()
        {
            // Simulate fetching real map coordinates.
            List<double> coordinatesValue = new()
            {
                22.4868527,
                88.350944
            };

            coordinatesValue = GetRandomCoordinate(coordinatesValue[0],coordinatesValue[1]);

            var coordinates = new CoordinatesModel
            {
                Latitude = coordinatesValue[0],
                Longitude = coordinatesValue[1]
            };
            return Json(coordinates);
        }

        //get random coordinates
        private static List<double> GetRandomCoordinate(double latitude, double longitude)
        {
            Random random = new Random();
            List<double> coordinatesValue = new()
            {
               latitude + (((random.NextDouble() * 180) - 90) / 100000),
               longitude +(((random.NextDouble() * 360) - 180) / 100000)
            };
            return coordinatesValue;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
