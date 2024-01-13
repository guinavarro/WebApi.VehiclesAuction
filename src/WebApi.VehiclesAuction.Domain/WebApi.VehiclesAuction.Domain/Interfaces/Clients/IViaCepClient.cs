using WebApi.VehiclesAuction.Domain.Clients;

namespace WebApi.VehiclesAuction.Domain.Interfaces.Clients
{
    public interface IViaCepClient
    {
        Task<ViaCepResult> FindByZip(string zipCode);
    }
}
