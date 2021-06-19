using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xTask.Core.Interfaces;
using xTask.Domain.Entities;

namespace xTask.Infrastructure.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        internal xTaskManagementContext _context;
        internal DbSet<TEntity> _dbSet;

        internal IUser _user; 
        

        public BaseRepository(xTaskManagementContext context, IUser user)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _user = user;
        }

        public async Task<TEntity> FindAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;

            entity.CreatedBy = _user.GetUserName();
            entity.ModifiedBy = _user.GetUserName();

            await _dbSet.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;
        }

        public async System.Threading.Tasks.Task DeleteAsync(int id,  CancellationToken cancellationToken = default)
        {
            TEntity entityToDelete = (TEntity)Activator.CreateInstance(typeof(TEntity));
            entityToDelete.ID = id;

            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
         
            await _context.SaveChangesAsync();
        }


        public async Task<TEntity> UpdateAsync(TEntity entityToUpdate,  CancellationToken cancellationToken = default)
        {
            //get do valor date created e user
            Tuple<DateTime, string> aux = (from cursor in _dbSet
                             where cursor.ID == entityToUpdate.ID
                             select new Tuple<DateTime, string>(cursor.CreatedOn, cursor.CreatedBy)
                                 ).FirstOrDefault();

            if (aux != null)
            {
                entityToUpdate.CreatedBy = aux.Item2;
                entityToUpdate.CreatedOn = aux.Item1;
            }

            entityToUpdate.ModifiedBy = _user.GetUserName();
            entityToUpdate.ModifiedOn = DateTime.Now;

            _context.Entry(entityToUpdate).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return entityToUpdate;
        }

        public IQueryable<TEntity> RawSql(string query, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters);

        }


        public IQueryable<TEntity> AsQueryable()
        {
            return _dbSet.AsNoTracking();
        }

    }

}
