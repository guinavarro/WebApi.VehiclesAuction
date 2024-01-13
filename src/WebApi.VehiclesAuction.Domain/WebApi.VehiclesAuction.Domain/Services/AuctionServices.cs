using WebApi.VehiclesAuction.Domain.Interfaces.Clients;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Models;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Domain.Models.Enums;
using WebApi.VehiclesAuction.Domain.Models.Models;

namespace WebApi.VehiclesAuction.Domain.Services
{
    public class AuctionServices : IAuctionServices
    {
        private readonly IBidRepository _bidRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAuctionItemRepository _auctionItemRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IEmailSender _emailSender;

        public AuctionServices(IBidRepository bidRepository,
            IAuctionRepository auctionRepository,
            IAuctionItemRepository auctionItemRepository,
            IItemRepository itemRepository,
            IEmailSender emailSender)
        {
            _bidRepository = bidRepository;
            _auctionRepository = auctionRepository;
            _auctionItemRepository = auctionItemRepository;
            _itemRepository = itemRepository;
            _emailSender = emailSender;
        }

        public async Task<Response<bool>> RegisterAuction(string name, DateTime startAt, DateTime endAt, bool isActive, CancellationToken cancellationToken = default)
        {
            try
            {
                var isDatesValid = DateValidator(startAt, endAt);

                if (!isDatesValid.IsValid)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a operação. Por favor, verifique os campos de data e tente novamente. Erro: {isDatesValid.Message}" });

                var isNameValid = StringValidator(name, 100);

                if (!isNameValid.isValid)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a operação. Por favor, verifique o campo de nome e tente novamente. Erro: {isDatesValid.Message}" });


                var auction = new Auction(name, startAt, endAt, isActive);
                var createAuction = await _auctionItemRepository.Add(auction, cancellationToken);

                if (!createAuction)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro na hora de tentar realizar a operação. Por favor, tente novamente mais tarde..." });

                return new Response<bool>(true, $"{name} cadastrado com sucesso. Com início previsto {startAt} e fim {endAt}.");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<List<AuctionModel>>> GetAllAuctions(CancellationToken cancellationToken = default)
        {
            try
            {
                var auctionsList = new List<AuctionModel>();
                var auctions = await _auctionRepository.GetAll(cancellationToken);

                if (!auctions.Any())
                    return new Response<List<AuctionModel>>(false, new List<string> { "Nenhum Leilão encontrado." });

                foreach (var auction in auctions)
                {
                    var auctionModel = new AuctionModel
                    {
                        AuctionName = auction.Name,
                        AuctionKey = auction.Key,
                        AuctionStartAt = auction.StartAt.ToString("dd/MM/yyyy"),
                        AuctionEndAt = auction.EndAt.ToString("dd/MM/yyyy"),
                        AuctionIsActive = auction.Active
                    };

                    if (auction.Items.Any())
                    {
                        foreach (var item in auction.Items)
                        {
                            var auctionItemModel = new AuctionItemModel
                            {
                                AuctionItemKey = item.Key,
                                AuctionItemName = item.Item.Name,
                                AuctionItemDescription = item.Item.Description,
                                AuctionItemBrand = item.Item.Brand,
                                AuctionItemType = (int)item.Item.Type,
                                AuctionItemMinimumBid = item.MinimumBid,
                                AuctionItemStartAtHours = item.StartAtHours,
                                AuctionItemEndAtHours = item.EndAtHours,
                            };

                            auctionModel.Items.Add(auctionItemModel);
                        }
                    }

                    auctionsList.Add(auctionModel);
                }

                return new Response<List<AuctionModel>>(true, auctionsList);
            }
            catch (Exception ex)
            {
                return new Response<List<AuctionModel>>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<bool>> UpdateAuctionStatus(Guid auctionKey, bool newActiveStatus, CancellationToken cancellationToken = default)
        {
            try
            {
                if (auctionKey == Guid.Empty)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${auctionKey} é inválida." });

                var getAuction = await _auctionRepository.GetAuctionByKey(auctionKey, cancellationToken);

                if (getAuction is null)
                    return new Response<bool>(false, "Leilão não encontrado.");

                var auction = getAuction;
                auction!.Active = newActiveStatus;

                var auctionUpdate = await _auctionRepository.Update(auction);

                if (!auctionUpdate)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro na hora de tentar realizar a operação. Por favor, tente novamente mais tarde..." });


                return new Response<bool>(true, $"{auction.Name} status atualizado com sucesso!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<AuctionModel>> GetAuctionByKey(Guid auctionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (auctionKey == Guid.Empty)
                    return new Response<AuctionModel>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${auctionKey} é inválida." });

                var auction = await _auctionRepository.GetAuctionByKey(auctionKey, cancellationToken);

                if (auction is null)
                    return new Response<AuctionModel>(false, new List<string> { "Nenhum Leilão encontrado." });

                var auctionModel = new AuctionModel
                {
                    AuctionName = auction.Name,
                    AuctionKey = auctionKey,
                    AuctionStartAt = auction.StartAt.ToString("dd/MM/yyyy"),
                    AuctionEndAt = auction.EndAt.ToString("dd/MM/yyyy"),
                    AuctionIsActive = auction.Active
                };

                if (auction.Items.Any())
                {
                    foreach (var item in auction.Items)
                    {
                        var auctionItemModel = new AuctionItemModel
                        {
                            AuctionItemKey = item.Key,
                            AuctionItemName = item.Item.Name,
                            AuctionItemDescription = item.Item.Description,
                            AuctionItemBrand = item.Item.Brand,
                            AuctionItemType = (int)item.Item.Type,
                            AuctionItemMinimumBid = item.MinimumBid,
                            AuctionItemStartAtHours = item.StartAtHours,
                            AuctionItemEndAtHours = item.EndAtHours,
                        };

                        auctionModel.Items.Add(auctionItemModel);
                    }
                }

                return new Response<AuctionModel>(true, auctionModel);

            }
            catch (Exception ex)
            {
                return new Response<AuctionModel>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<bool>> RegisterAuctionItems(Guid auctionKey, List<AuctionItemModel> items, CancellationToken cancellationToken = default)
        {

            try
            {
                if (auctionKey == Guid.Empty)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${auctionKey} é inválida." });

                var errorList = new List<string>();
                var getAuction = await _auctionRepository.GetAuctionByKey(auctionKey, cancellationToken);

                if (getAuction is null)
                    return new Response<bool>(false, new List<string> { "Leilão não encontrado" });


                var auction = getAuction;

                if (auction.EndAt.Date < DateTime.Now.Date)
                    return new Response<bool>(false, new List<string> { $"Não foi possível cadastrar os itens. Esse leilão se encerrou no dia {auction.EndAt.Date.ToString("dd/MM/yyyy")}" });

                if (!items.Any())
                    return new Response<bool>(false, new List<string> { "Erro ao tentar realizar a operação. É necessário cadastrar pelo menos um item." });

                foreach (var obj in items)
                {
                    var isAuctionItemPeriodValid = ValidateItemAuctionDate(auction.StartAt, auction.EndAt, obj.AuctionItemStartAtHours, obj.AuctionItemEndAtHours, obj.AuctionItemName);

                    if (!isAuctionItemPeriodValid.isValid)
                    {
                        errorList.Add(isAuctionItemPeriodValid.Message);
                        return new Response<bool>(false, errorList);
                    }

                    var item = new Item(obj.AuctionItemName, obj.AuctionItemDescription, obj.AuctionItemBrand, (VehicleType)obj.AuctionItemType);
                    var createItem = await _itemRepository.Add(item, cancellationToken);

                    if (!createItem)
                        errorList.Add($"Erro ao tentar adicionar o item {item}.");

                    var auctionItem = new AuctionItem(auction.Id, item.Id, obj.AuctionItemMinimumBid, obj.AuctionItemStartAtHours, obj.AuctionItemEndAtHours);
                    var createAuctionItem = await _auctionItemRepository.Add(auctionItem, cancellationToken);

                    if (!createAuctionItem)
                        errorList.Add($"Erro ao tentar cadastrar o item {item} como leiloável.");
                }

                if (errorList.Any())
                    return new Response<bool>(false, errorList);


                return new Response<bool>(true, $"{items.Count} itens foram cadastrados com sucesso para {auction.Name}");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<bool>> BidItemAuction(Guid itemAuctionKey, decimal bidValue, int participantId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (itemAuctionKey == Guid.Empty)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${itemAuctionKey} é inválida." });

                if (bidValue <= 0)
                    return new Response<bool>(false, new List<string> { "É necessário realizar o lance com um valor válido. Só é permitido valores acima de R$0,00" });

                var auctionItem = await _auctionItemRepository.GetAuctionItemByKey(itemAuctionKey, cancellationToken);

                if (auctionItem is null)
                    return new Response<bool>(false, new List<string> { "Item não encontrado." });

                var auction = auctionItem.Auction;
                var bidsList = auctionItem.Bids;
                var currentDate = DateTime.Now;
                var currenteDateTimeSpan = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);

                if (bidsList.Any(x => x.Winner == true))
                    return new Response<bool>(false, "Não foi possível realizar o lance, pois esse item já foi leiloado e possui um vencedor.");

                if (!ValidateIfAuctionIsHappening(auction.StartAt, auction.EndAt, currentDate))
                    return new Response<bool>(false, new List<string> { $"Não é possível mais realizar lances para esse item, pois o {auction.Name} se encerrou no dia {auction.EndAt.ToString("dd/MM/yyyy")}" });

                if (!VerifyIfAuctionItemIsOcurring(auctionItem.StartAtHours, auctionItem.EndAtHours, currenteDateTimeSpan))
                    return new Response<bool>(false, new List<string> { $"Não foi possível realizar o lance. Confira o horário do leilão desse item." });

                var minimumBid = auctionItem.MinimumBid;
                var currentItemValue = auctionItem.CurrentValue;

                var createBid = false;
                var updateItemAuction = false;

                /// Se não tiver nenhum lance feito no item, realize o primeiro lance
                if (!bidsList.Any())
                {
                    if (currentItemValue != null && bidValue < currentItemValue)
                    {
                        return new Response<bool>(false, new List<string> { $"Seu lance R${bidValue} foi abaixo do valor mínimo atual R${currentItemValue}." });
                    }

                    var bid = new Bid(participantId, auctionItem.Id, bidValue);
                    createBid = await _bidRepository.Add(bid, cancellationToken);

                    if (!createBid)
                        return new Response<bool>(false, new List<string> { "Ocorreu um erro interno na hora de realizar o lance. Por favor, tente novamente." });

                    auctionItem.UpdateCurrentValue(bidValue);
                    updateItemAuction = await _auctionItemRepository.Update(auctionItem, cancellationToken);

                    if (!updateItemAuction)
                        return new Response<bool>(false, new List<string> { "Ocorreu um erro interno na hora de realizar o lance. Por favor, tente novamente." });
                }

                if (bidValue < currentItemValue)
                    return new Response<bool>(false, new List<string> { $"Seu lance R${bidValue} foi abaixo do valor mínimo atual R${currentItemValue}." });


                bidsList = bidsList.OrderBy(x => x.CreatedAt).ToList();
                var lastGreaterBid = bidsList.OrderByDescending(x => x.Value).FirstOrDefault();

                if (lastGreaterBid!.ParticipantId == participantId)
                    return new Response<bool>(false, new List<string> { $"Lance não realizado. Seu último lance de R${lastGreaterBid.Value} continua sendo o maior." });

                if (bidValue <= lastGreaterBid.Value)
                    return new Response<bool>(false, new List<string> { $"Seu lance R${bidValue} foi menor que o lance de outro participante." });

                var newBid = new Bid(participantId, auctionItem.Id, bidValue);
                createBid = await _bidRepository.Add(newBid, cancellationToken);

                if (!createBid)
                    return new Response<bool>(false, new List<string> { "Ocorreu um erro interno na hora de realizar o lance. Por favor, tente novamente." });

                auctionItem.UpdateCurrentValue(bidValue);
                updateItemAuction = await _auctionItemRepository.Update(auctionItem, cancellationToken);

                if (!updateItemAuction)
                    return new Response<bool>(false, new List<string> { "Ocorreu um erro interno na hora de realizar o lance. Por favor, tente novamente." });


                return new Response<bool>(true, $"O lance no valor de R${bidValue} foi realizado com sucesso!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        public async Task<Response<bool>> RemoveAuctionItem(Guid itemAuctionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (itemAuctionKey == Guid.Empty)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${itemAuctionKey} é inválida." });

                var itemAuction = await _auctionItemRepository.GetAuctionItemByKey(itemAuctionKey, cancellationToken);

                if (itemAuction == null)
                    return new Response<bool>(false, new List<string> { "Item não encontrado." });

                if (itemAuction.Bids.Any())
                    return new Response<bool>(false, new List<string> { "Não é mais possível remover esse item, pois lances já foram feitos." });

                var removeItemAuction = await _auctionItemRepository.Delete(itemAuction, cancellationToken);

                if (!removeItemAuction)
                    return new Response<bool>(false, new List<string> { "Houve um erro na hora de tentar remover o Item. Por favor, tente novamente" });


                return new Response<bool>(true, "Item removido com sucesso!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }


        public async Task<Response<bool>> RemoveAuction(Guid auctionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                if (auctionKey == Guid.Empty)
                    return new Response<bool>(false, new List<string> { $"Ocorreu um erro ao tentar realizar a busca. A chave é ${auctionKey} é inválida." });

                var auction = await _auctionRepository.GetAuctionByKey(auctionKey, cancellationToken);

                if (auction == null)
                    return new Response<bool>(false, new List<string> { "Leilão não encontrado." });

                if (auction.StartAt.Date >= DateTime.Now.Date)
                    return new Response<bool>(false, new List<string> { "Não foi possível remover esse Leilão, pois ele já iniciou" });

                if (auction.Items.Any(x => x.Bids.Any()))
                    return new Response<bool>(false, new List<string> { "Não foi possível remover esse Leilão, pois existem itens que já foram feitos lances." });

                var removeAuction = await _auctionRepository.Delete(auction, cancellationToken);

                if (!removeAuction)
                    return new Response<bool>(false, new List<string> { "Houve um erro na hora de tentar remover o Leilão. Por favor, tente novamente" });


                return new Response<bool>(true, "Leilão removido com sucesso!");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operação. Por favor, entre em contato com o suporte. Erro {ex.Message.ToString()}" });
            }
        }

        // Criar um dos servicos que bate minuto a minuto pra ver os lances

        public async Task NotifyDailyWinners()
        {
            try
            {
                //var date = DateTime.Now.AddDays(1);
                var date = DateTime.Now;
                var bids = await _bidRepository.GetBidWinnersByDate(date);

                if (bids != null && bids.Any())
                {
                    foreach (var bid in bids)
                    {
                        var winnerParticipant = bid.Participant;

                        var email = winnerParticipant.Email;
                        var name = winnerParticipant.Name;
                        var subject = $"Você venceu o leilão do {bid.AuctionItem.Item.Name}...";
                        var message = @$"Parabéns {name}, você venceu o leilão do item
                        {bid.AuctionItem.Item.Name}.
                        Você tem 3 dias úteis para realizar o pagamento do lance R${bid.Value}";

                        email = "marq@mailinator.com";

                        await _emailSender.SendEmail(subject, email, name, message);
                    }
                }
            }
            catch
            {

            }
        }

        public async Task DailyAuctionsAudit()
        {
            try
            {
                var auctionsItem = await _auctionItemRepository.GetAuctionsItem();
                auctionsItem = auctionsItem.Where(x => x.Auction.EndAt.Date >= DateTime.Now.Date).ToList();

                var currentDate = DateTime.Now;
                var currenteDateTimeSpan = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);

                if (auctionsItem != null && auctionsItem.Any())
                {
                    foreach (var auctionItem in auctionsItem)
                    {
                        // Verifica se já se encerrou o leilão daquele item
                        if (DateTime.Now.Date >= auctionItem.Auction.EndAt.Date
                            && currenteDateTimeSpan >= auctionItem.EndAtHours)
                        {
                            // Verifica se teve lances para aquele item
                            if (auctionItem.Bids != null && auctionItem.Bids.Any())
                            {
                                // Pega o último lance de valor mais alto e define como vencedor
                                var higherBid = auctionItem.Bids.OrderByDescending(x => x.Value).FirstOrDefault();

                                higherBid!.Winner = true;
                                await _auctionItemRepository.Update(higherBid);
                            }
                        }
                    }
                }

            }
            catch
            {
                // TODO: adicionar Hangfire.Console;
            }
        }


        #region Validators
        private (bool isValid, string Message) ValidateItemAuctionDate(DateTime auctionStartDate, DateTime auctionEndDate, TimeSpan itemAuctionStart, TimeSpan itemAuctionEnd, string itemName)
        {

            if (itemAuctionEnd < itemAuctionStart)
            {
                return (false, $"O horário de términio do leilão do item {itemName} é menor que o horário final do leilão do item ");
            }

            var startHours = auctionStartDate.TimeOfDay;
            var endHours = auctionEndDate.TimeOfDay;

            if (itemAuctionStart > endHours)
            {
                return (false, $"O horário de início do leilão do item {itemName} é maior que o horário final do leilão ");
            }
            else if (itemAuctionStart < startHours)
            {
                return (false, $"O horário de início do leilão do item {itemName} é anterior que o horário inicial do leilão");

            }
            else if (itemAuctionEnd < startHours)
            {
                return (false, $"O horário de término do leilão do item {itemName} é anterior que o horário inicial do leilão");
            }
            else if (itemAuctionEnd > endHours)
            {
                return (false, $"O horário de término do leilão do item {itemName} é maior que o horário final do leilão");
            }

            return (true, string.Empty);

        }
        private (bool isValid, string Message) StringValidator(string value, int? maximumLength = null)
        {
            if (string.IsNullOrEmpty(value))
                return (false, "O campo está vazio. Por favor, insira um valor válido");

            if (maximumLength != null && value.Length > maximumLength.Value)
                return (false, $"O campo excedeu o limite de caracteres permitido. O comprimento máximo é {maximumLength.Value} caracteres");

            return (true, string.Empty);
        }
        private (bool IsValid, string Message) DateValidator(DateTime firstDate, DateTime? secondDate)
        {
            if (firstDate.Date < DateTime.Now.Date)
            {
                return (false, "Data inválida. A data de início é anterior a data atual. Por favor, insira uma data futura.");
            }


            if (secondDate is not null)
            {
                if (secondDate.Value.Date < DateTime.Now.Date)
                {
                    return (false, "Data inválida. A data de fim é anterior a data atual. Por favor, insira uma data futura.");
                }

                if (secondDate.Value.Date < firstDate.Date)
                {
                    return (false, "Data inválida. A data de fim é anterior a data atual. Por favor, insira uma válida");
                }
            }


            return (true, string.Empty);
        }

        private bool ValidateIfAuctionIsHappening(DateTime auctionStart, DateTime auctionEnd, DateTime date)
        => date >= auctionStart && date <= auctionEnd;

        private bool VerifyIfAuctionItemIsOcurring(TimeSpan itemAuctionStart, TimeSpan itemAuctionEnd, TimeSpan bid)
            => bid >= itemAuctionStart && bid <= itemAuctionEnd;

        
    }

    #endregion
}

