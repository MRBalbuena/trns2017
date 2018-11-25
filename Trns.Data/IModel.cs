using System.Threading.Tasks;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

namespace Trns.Data
{
    namespace Trs.Data
    {
        public interface IModel
        {
            EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
            Database Database { get; }
            int SaveChanges();
            Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        }
    }
}
