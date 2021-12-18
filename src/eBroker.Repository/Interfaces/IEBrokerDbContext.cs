using eBroker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace eBroker.Data.Interfaces
{
    public interface IEBrokerDbContext
    {
        DbSet<T> Set<T>() where T : class;
        DbSet<Equity> Equities { get; set; }

        DbSet<Trader> Traders { get; set; }

        DbSet<OwnedEquity> OwnedEquities { get; set; }

        ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) where TEntity: class;
        EntityEntry Update(object entity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
