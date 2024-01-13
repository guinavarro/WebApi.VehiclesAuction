namespace WebApi.VehiclesAuction.Domain.Interfaces.Repository
{
    public interface IBaseRepository
    {
        Task<bool> Add<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task<bool> Update<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task<bool> Delete<T>(T entity, CancellationToken cancellationToken = default) where T : class;
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
