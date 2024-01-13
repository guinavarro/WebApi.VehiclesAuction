using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.VehiclesAuction.Domain.Interfaces.Clients;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuctionParticipantController : ControllerBase
    {
        private readonly IViaCepClient _viaCepClient;

        public AuctionParticipantController(IViaCepClient viaCepClient)
        {
            _viaCepClient = viaCepClient;
        }

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

        [HttpPost("cep")]
        public async Task<IActionResult> Cep([FromForm] string cep)
        {
            var teste = await _viaCepClient.FindByZip(cep);

            return Ok(teste);
        }
    }
}
