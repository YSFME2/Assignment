﻿using System.Linq.Expressions;

namespace Domain.Abstractions
{
    public interface IGenericRepository<TEntity> where TEntity : AuditableEntity
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter);
        Task AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        bool Delete(TEntity entity);
        Task<bool> Delete(int id);
    }
}
