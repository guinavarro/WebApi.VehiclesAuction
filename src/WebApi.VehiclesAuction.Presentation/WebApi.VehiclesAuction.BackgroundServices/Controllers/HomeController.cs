using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.VehiclesAuction.BackgroundServices.Models;

namespace WebApi.VehiclesAuction.BackgroundServices.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Home() => Redirect("/hangfire");
    }
}
