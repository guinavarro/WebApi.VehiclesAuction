using WebApi.VehiclesAuction.Domain.Models.Entities;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Repository
{
    public interface IParticipantRepository : IBaseRepository
    {
        Task<Participant> GetParticipantById(int id, CancellationToken cancellationToken = default);
        Task<Participant> GetParticipantByEmail(string email, CancellationToken cancellationToken = default);
    }
}
