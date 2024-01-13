using WebApi.VehiclesAuction.Domain.Interfaces.Clients;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;
using WebApi.VehiclesAuction.Domain.Models;
using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Domain.Services
{
    public class ParticipantServices : IParticipantServices
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IViaCepClient _viaCepClient;

        public ParticipantServices(IAddressRepository addressRepository, IParticipantRepository participantRepository, IViaCepClient viaCepClient)
        {
            _addressRepository = addressRepository;
            _participantRepository = participantRepository;
            _viaCepClient = viaCepClient;
        }

        public async Task<Response<bool>> AddParticipant(string name, string email, string cep, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    return new Response<bool>(false, new List<string> { "Ops, ocorreu um erro. O campo Nome é obrigatório."});

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
                return new Response<bool>(false, new List<string> { ex.Message });
            }
        }
    }
}
