using System.Threading;
using WebApi.VehiclesAuction.Domain.Interfaces.Repository;

namespace WebApi.VehiclesAuction.Infra.Repository
{
    public class BaseRepository : IBaseRepository
    {

        public BaseRepository(VehiclesAuctionContext context)
        {
            _context = context;
        }
        private readonly VehiclesAuctionContext _context;

        public async Task<bool> Add<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            try
            {
                _context.Add(entity);
                return await _context.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> Delete<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            try
            {
                _context.Remove(entity);
                return await _context.SaveChangesAsync(CancellationToken.None) > 0;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> Update<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            try
            {
                _context.Update(entity);
                return await _context.SaveChangesAsync(CancellationToken.None) > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.SaveChangesAsync(CancellationToken.None) > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
