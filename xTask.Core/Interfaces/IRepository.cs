using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace xTask.Core.Interfaces
{
    /// <summary>
    /// Pattern IRepository Contrat
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> AsQueryable();
        Task<TEntity> FindAsync(int id, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id,  CancellationToken cancellationToken = default);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> UpdateAsync(TEntity entityToUpdate,  CancellationToken cancellationToken = default);
    }
}
