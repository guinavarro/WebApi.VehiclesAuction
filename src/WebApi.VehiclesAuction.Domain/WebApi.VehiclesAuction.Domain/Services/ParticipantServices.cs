using WebApi.VehiclesAuction.Domain.Interfaces.Repository;
using WebApi.VehiclesAuction.Domain.Interfaces.Services;

namespace WebApi.VehiclesAuction.Domain.Services
{
    public class ParticipantServices : IParticipantServices
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IParticipantRepository _participantRepository;

        public ParticipantServices(IAddressRepository addressRepository, IParticipantRepository participantRepository)
        {
            _addressRepository = addressRepository;
            _participantRepository = participantRepository;
        }

        public Task<bool> AddParticipant()
        {
            throw new NotImplementedException();
        }
    }
}
