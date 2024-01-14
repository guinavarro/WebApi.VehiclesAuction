using Microsoft.AspNetCore.Mvc;
using WebApi.VehiclesAuction.Api.Models;
using WebApi.VehiclesAuction.Domain.Interfaces.Clients;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TesteController : Controller
    {
        private readonly IViaCepClient _viaCepClient;

        public TesteController(IViaCepClient viaCepClient)
        {
            _viaCepClient = viaCepClient;
        }

        ///<remarks>
        /// Busca o CEP na base do ViaCep
        /// </remarks>
        /// <summary>
        /// Busca CEP
        /// </summary>
        /// <param name="cep">Parâmetros para cadastro de usuário Admin.</param>
        /// <response code="200">CEP localizado com sucesso</response>
        /// <returns></returns>
        [ProducesResponseType(typeof(JsonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("viacep/buscar-cep")]
        public async Task<IActionResult> Cep(string cep)
        {
            var teste = await _viaCepClient.FindByZip(cep);
            return Ok(teste);
        }
    }
}
