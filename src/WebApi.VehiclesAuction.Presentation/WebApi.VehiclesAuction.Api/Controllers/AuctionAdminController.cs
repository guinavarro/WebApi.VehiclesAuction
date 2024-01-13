﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.VehiclesAuction.Api.Models;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Domain.Models.Enums;
using WebApi.VehiclesAuction.Domain.Models.Models;

namespace WebApi.VehiclesAuction.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuctionAdminController : ControllerBase
    {
        private readonly IAuctionServices _auctionServices;

        public AuctionAdminController(IAuctionServices auctionServices)
        {
            _auctionServices = auctionServices;
        }


        [HttpPost("register-auction")]
        public async Task<IActionResult> RegisterAuction([FromBody] RegisterAuctionViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. Por favor, verifique os campos e tente novamente."));

            var createAuction = await _auctionServices.RegisterAuction(viewModel.Name, viewModel.StartAt, viewModel.EndAt, viewModel.IsActive, cancellationToken);

            if (!createAuction.Success)
                return BadRequest(new JsonResponse(false, createAuction.GetErrorMessage()));

            return Ok(new JsonResponse(true, "Leilão cadastrado com sucesso."));
        }
        [HttpPost("register-auction-items")]
        public async Task<IActionResult> RegisterAuctionItems([FromBody] RegisterAuctionItemsViewModel viewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. Por favor, verifique os campos e tente novamente."));

            if (!viewModel.AuctionItems.Any())
                return BadRequest(new JsonResponse(false, "Erro ao tentar realizar o cadastro. É necessário cadastrar pelo menos um item para Leilão."));

            var auctionItemsList = new List<AuctionItemModel>();

            foreach (var viewModelItem in viewModel.AuctionItems)
            {
                var isStartHoursValid = ValidateHour(viewModelItem.StartHours);
                var isStartMinutesValid = ValidateMinutes(viewModelItem.StartMinutes);

                if (!isStartHoursValid || !isStartMinutesValid)
                    return BadRequest(new JsonResponse(false, $"Horário de início para o leilão inválido para o item {viewModelItem.Name}. Insira um valor inteiro para horas entre 0 e 23 e para minutos entre 0 e 59"));

                var isEndHoursValid = ValidateHour(viewModelItem.EndHours);
                var isSEndMinutesValid = ValidateMinutes(viewModelItem.EndMinutes);

                if (!isEndHoursValid || !isSEndMinutesValid)
                    return BadRequest(new JsonResponse(false, $"Horário de encerramento para o leilão inválido para o item {viewModelItem.Name}. Insira um valor inteiro para horas entre 0 e 23 e para minutos entre 0 e 59"));

                if (!IsVehicleTypeValid(viewModelItem.Type))
                    return BadRequest(new JsonResponse(false, $"Tipo de Veículo inexistente para o item {viewModelItem.Name}"));

                var item = new AuctionItemModel
                {
                    AuctionItemName = viewModelItem.Name,
                    AuctionItemBrand = viewModelItem.Brand,
                    AuctionItemDescription = viewModelItem.Description,
                    AuctionItemStartAtHours = new TimeSpan(viewModelItem.StartHours, viewModelItem.StartMinutes, 0),
                    AuctionItemEndAtHours = new TimeSpan(viewModelItem.EndHours, viewModelItem.EndMinutes, 0),
                    AuctionItemType = viewModelItem.Type,
                    AuctionItemMinimumBid = viewModelItem.MinimumBid,
                };

                auctionItemsList.Add(item);
            }

            var createAuctionItems = await _auctionServices.RegisterAuctionItems(viewModel.AuctionKey, auctionItemsList, cancellationToken);

            if (!createAuctionItems.Success)
                return BadRequest(new JsonResponse(false, createAuctionItems.GetAllErrorsMessage()));


            return Ok(new JsonResponse(true, createAuctionItems.Message!));
        }

        [HttpGet("get-auction/{key}")]
        public async Task<IActionResult> GetAuctionByKey(Guid key, CancellationToken cancellationToken)
        {
            var getAuction = await _auctionServices.GetAuctionByKey(key, cancellationToken);

            if (!getAuction.Success)
                return BadRequest(new JsonResponse(false, getAuction.GetErrorMessage()));


            return Ok(getAuction.Object!);
        }
        [HttpGet("get-all-auctions")]
        public async Task<IActionResult> GetAllAuctions(CancellationToken cancellationToken)
        {
            var getAuctions = await _auctionServices.GetAllAuctions(cancellationToken);

            if (!getAuctions.Success)
                return BadRequest(new JsonResponse(false, getAuctions.GetErrorMessage()));

            return Ok(getAuctions.Object!);
        }

        [HttpDelete("remove-auction-item/{key}")]
        public async Task<IActionResult> RemoveAuctionItem(Guid key, CancellationToken cancellationToken)
        {
            var removeAuctionItem = await _auctionServices.RemoveAuctionItem(key, cancellationToken);

            if (!removeAuctionItem.Success)
                return BadRequest(new JsonResponse(false, removeAuctionItem.GetErrorMessage()));

            return Ok(removeAuctionItem.Message);
        }

        [HttpDelete("remove-auction/{key}")]
        public async Task<IActionResult> RemoveAuction(Guid key, CancellationToken cancellationToken)
        {
            var removeAuction = await _auctionServices.RemoveAuction(key, cancellationToken);

            if (!removeAuction.Success)
                return BadRequest(new JsonResponse(false, removeAuction.GetErrorMessage()));

            return Ok(removeAuction.Message);
        }

        #region Métodos Privados
        private bool IsVehicleTypeValid(int valor)
        {
            int[] valoresValidos = (int[])Enum.GetValues(typeof(VehicleType));

            return Array.IndexOf(valoresValidos, valor) != -1;
        }
        private bool ValidateHour(int hour) =>
            hour >= 0 && hour <= 23;  
        private bool ValidateMinutes(int minutes) =>
            minutes >= 0 && minutes <= 59;
        #endregion
    }
}
