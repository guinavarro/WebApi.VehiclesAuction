using WebApi.VehiclesAuction.Domain.Models;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Services
{
    public interface IParticipantServices
    {
        Task<Response<bool>> AddParticipant(string name, string email, string cep, CancellationToken cancellationToken = default);
    }
}
