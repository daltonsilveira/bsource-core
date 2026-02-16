using BSourceCore.Application.Abstractions;
using BSourceCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BSourceCore.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly WriteDbContext _writeContext;
    protected readonly ReadOnlyDbContext _readContext;
    protected readonly DbSet<T> _writeDbSet;
    protected readonly DbSet<T> _readDbSet;

    public Repository(WriteDbContext writeContext, ReadOnlyDbContext readContext)
    {
        _writeContext = writeContext;
        _readContext = readContext;
        _writeDbSet = writeContext.Set<T>();
        _readDbSet = readContext.Set<T>();
    }

    // === READ Operations (usando ReadOnlyDbContext) ===
    
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _readDbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _readDbSet.ToListAsync(cancellationToken);
    }

    // === WRITE Operations (usando WriteDbContext) ===
    
    public virtual async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _writeDbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public virtual void Update(T entity)
    {
        _writeDbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _writeDbSet.Remove(entity);
    }
}
