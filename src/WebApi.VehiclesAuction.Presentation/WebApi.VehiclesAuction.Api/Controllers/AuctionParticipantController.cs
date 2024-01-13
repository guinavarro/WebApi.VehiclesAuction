using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuctionParticipantController : ControllerBase
    {
        [HttpGet("example")]
        [Authorize(Roles = "Participant")]

        public IActionResult Example()
        {

            var username = User.Identity.Name;
            return Ok(new
            {
                Message = "Você está autenticado como Participant!",
                Username = username
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {

            var username = User.Identity.Name;
            return Ok(new
            {
                Message = "Você está autenticado como admin!",
                Username = username
            });
        }
    }
}
