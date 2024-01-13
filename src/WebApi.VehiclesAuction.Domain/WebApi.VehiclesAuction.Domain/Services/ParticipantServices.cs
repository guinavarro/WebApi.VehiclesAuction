using WebApi.VehiclesAuction.Domain.Interfaces.Clients;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Models;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Domain.Models.Models;

namespace WebApi.VehiclesAuction.Domain.Services
{
    public class ParticipantServices : IParticipantServices
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IViaCepClient _viaCepClient;
        private readonly IAuctionServices _auctionServices;

        public ParticipantServices(IAddressRepository addressRepository,
            IParticipantRepository participantRepository,
            IViaCepClient viaCepClient,
            IAuctionServices auctionServices)
        {
            _addressRepository = addressRepository;
            _participantRepository = participantRepository;
            _viaCepClient = viaCepClient;
            _auctionServices = auctionServices;
        }

        public async Task<Response<bool>> AddParticipant(string name, string email, string cep, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro. O campo Nome é obrigatório." });

                if (string.IsNullOrEmpty(email))
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro. O campo Nome é obrigatório." });

                if (string.IsNullOrEmpty(cep))
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro. O campo Nome é obrigatório." });


                var viaCepResponse = await _viaCepClient.FindByZip(cep);

                if (viaCepResponse is null)
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro interno ao tentar buscar o seu CEP. Tente novamente mais tarde." });

                var address = new Address(viaCepResponse.Logradouro, "", viaCepResponse.Bairro, viaCepResponse.Cep, viaCepResponse.Localidade);
                var createAddress = await _addressRepository.Add(address, cancellationToken);

                if (!createAddress)
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro interno. Tente novamente mais tarde." });

                var participant = new Participant(name, email, address.Id);
                var createParticipant = await _participantRepository.Add(participant, cancellationToken);

                if (!createParticipant)
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro interno. Tente novamente mais tarde." });


                return new Response<bool>(true, "Usuário cadastrado com sucesso");
            }
            catch (Exception ex)
            {
                return new Response<bool>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operçaão. Erro: {ex.Message}" });
            }
        }

        public async Task<Response<AddressModel>> GetAddressByParticipantId(int participantId, CancellationToken cancellationToken = default)
        {
            try
            {
                var participant = await _participantRepository.GetParticipantById(participantId, cancellationToken);

                if (participant == null)
                    return new Response<AddressModel>(false, new List<string> { "Participante não encontrado." });

                var addressModel = new AddressModel
                {
                    Street = participant.Address.Street,
                    City = participant.Address.City,
                    Number = participant.Address.Number,
                    District = participant.Address.District,
                    Cep = participant.Address.Cep,
                };

                return new Response<AddressModel>(true, addressModel);
            }
            catch (Exception ex)
            {
                return new Response<AddressModel>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operçaão. Erro: {ex.Message}" });
            }
        }

        public async Task<Response<List<AuctionModel>>> GetAvailablesAuction(CancellationToken cancellationToken = default)
        {
            try
            {
                var getAuctions = await _auctionServices.GetAllAuctions(cancellationToken);

                if (!getAuctions.Success)
                    return new Response<List<AuctionModel>>(false, getAuctions.Message);

                var auctions = getAuctions.Object!.Where(x => x.AuctionIsActive == true).ToList();

                return new Response<List<AuctionModel>>(true, auctions);
            }
            catch (Exception ex)
            {
                return new Response<List<AuctionModel>>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operçaão. Erro: {ex.Message}" });
            }
        }

        public async Task<Response<int>> GetParticipantIdByEmail(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                    return new Response<int>(false, new List<string> { "Ops, ocorreu um erro. O campo email é obrigatório." });

                var participant = await _participantRepository.GetParticipantByEmail(email, cancellationToken);

                if (participant == null)
                    return new Response<int>(false, new List<string> { "Participante não encontrado." });

                return new Response<int>(true, participant.Id);
            }
            catch (Exception ex)
            {
                return new Response<int>(false, new List<string> { $"Ocorreu um erro interno ao tentar realizar a operçaão. Erro: {ex.Message}" });
            }
        }
    }
}
