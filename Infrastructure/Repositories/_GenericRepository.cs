using Domain.Abstractions;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : AuditableEntity
    {
        protected readonly AppDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected IQueryable<TEntity> _queryable;

        public GenericRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
            _queryable = _dbSet.AsQueryable().Where(x => !x.IsDeleted);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
            => await _queryable.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => _queryable;

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
            => _queryable.Where(filter).AsQueryable();

        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
            => await _queryable.AnyAsync(filter);

        public async Task<bool> Delete(int id)
        {
            var entity = await _queryable.FirstOrDefaultAsync(x => x.Id == id);
            if (entity != null)
            {
                entity.IsDeleted = true;
                return true;
            }
            return false;
        }
    }
}
