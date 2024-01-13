using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.VehiclesAuction.Api.Models;
using WebApi.VehiclesAuction.Domain.Interfaces.Clients;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Participant")]
    public class AuctionParticipantController : ControllerBase
    {
        private readonly IViaCepClient _viaCepClient;
        private readonly IAuctionServices _auctionServices;
        private readonly IParticipantServices _participantServices;

        public AuctionParticipantController(IViaCepClient viaCepClient,
        IAuctionServices auctionServices,
        IParticipantServices participantServices)
        {
            _viaCepClient = viaCepClient;
            _auctionServices = auctionServices;
            _participantServices = participantServices;
        }

        [HttpGet("get-all-availables-auction")]
        public async Task<IActionResult> GetAllAvailablesAuction(CancellationToken cancellationToken)
        {
            var getAvailablesAuction = await _participantServices.GetAvailablesAuction(cancellationToken);

            if (!getAvailablesAuction.Success)
                return BadRequest(new JsonResponse(false, getAvailablesAuction.GetErrorMessage()));

            if (!getAvailablesAuction.Object.Any())
                return Ok(new JsonResponse(true, "Nenhum leilão ativo no momento."));

            return Ok(getAvailablesAuction.Object);
        }


        [HttpPost("bid-auction-item")]
        public async Task<IActionResult> BidItem([FromForm] BidViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. Por favor, verifique os campos e tente novamente."));

            var participantId = GetParticipantIdByClaim();
            var createBid = await _auctionServices.BidItemAuction(viewModel.AuctionItemKey, viewModel.Value, participantId, cancellationToken);

            if (!createBid.Success)
                return BadRequest(new JsonResponse(false, createBid.GetErrorMessage()));

            return Ok(new JsonResponse(true, createBid.Message!));
        }

        //[HttpPost("viacep/teste")]
        //public async Task<IActionResult> Cep([FromForm] string cep)
        //{
        //    var teste = await _viaCepClient.FindByZip(cep);

        //    return Ok(teste);
        //}

        [HttpPost("email/teste")]
        public async Task<IActionResult> Teste()
        {
           await _auctionServices.NotifyDailyWinners();

            return Ok();
        }

        [HttpGet("get-authenticated-participant-address")]
        public async Task<IActionResult> GetAuthenticatedParticipantAddress(CancellationToken cancellationToken)
        {
            var participantId = GetParticipantIdByClaim();
            var getAddress = await _participantServices.GetAddressByParticipantId(participantId, cancellationToken);

            if (!getAddress.Success)
                return BadRequest(new JsonResponse(false, getAddress.GetErrorMessage()));

            return Ok(getAddress.Object);
        }

        private int GetParticipantIdByClaim()
        {
            var claims = User.Claims.ToList();
            var claimValue = User.FindFirst("ParticipantId")?.Value;

            return Convert.ToInt32(claimValue);
        }
    }
}
