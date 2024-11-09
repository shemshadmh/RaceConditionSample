using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace RaceConditionSample.Interface;

public interface IApplicationDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

    int SaveChanges(bool acceptAllChangesOnSuccess);
    int SaveChanges();
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
}