namespace Adasit.Foundation.Domain.Queries;

public interface ISearchableRepository<TEntity, TEntityId>
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);

    Task<SearchOutput<TEntity>> SearchAsync<TSearchInput>(TSearchInput input,
        CancellationToken cancellationToken);
}
