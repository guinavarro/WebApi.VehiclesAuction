using System.Threading;
using WebApi.VehiclesAuction.Domain.Models;
using WebApi.VehiclesAuction.Domain.Models.Entities;
using WebApi.VehiclesAuction.Domain.Models.Models;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Services
{
    public interface IParticipantServices
    {
        Task<Response<bool>> AddParticipant(string name, string email, string cep, CancellationToken cancellationToken = default);
        Task<Response<int>> GetParticipantIdByEmail(string email, CancellationToken cancellationToken = default);
        Task<Response<AddressModel>> GetAddressByParticipantId(int participantId, CancellationToken cancellationToken = default);
        Task<Response<List<AuctionModel>>> GetAvailablesAuction(CancellationToken cancellationToken = default);
    }
}
