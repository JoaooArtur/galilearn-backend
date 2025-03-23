﻿using Core.Domain.Projection;
using Core.Persistence.Projection.Abstractions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Core.Persistence.Projection
{
    public class Projection<TProjection>(IMongoDbContext context) : IProjection<TProjection>
        where TProjection : Domain.Primitives.IProjection
    {
        private readonly IMongoCollection<TProjection> _collection = context.GetCollection<TProjection>();

        public Task<TProjection> GetAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : struct
            => FindAsync(projection => projection.Id.Equals(id), cancellationToken);

        public Task<TProjection> FindAsync(Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken = default)
            => _collection.AsQueryable().Where(predicate).FirstOrDefaultAsync(cancellationToken)!;

        public Task<List<TProjection>> ListAsync(CancellationToken cancellationToken = default)
            => _collection.AsQueryable().ToListAsync(cancellationToken: cancellationToken);

        public Task<List<TProjection>> ListAsync(Expression<Func<TProjection, bool>> predicate, CancellationToken cancellationToken = default)
            => _collection.AsQueryable().Where(predicate).ToListAsync(cancellationToken: cancellationToken);

        public Task DeleteAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : struct
            => _collection.DeleteOneAsync(projection => projection.Id.Equals(id), cancellationToken);

        public Task DeleteAsync(Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken = default)
            => _collection.DeleteManyAsync(filter, cancellationToken);

        public Task UpdateOneFieldAsync<TField>(Expression<Func<TProjection, bool>> filter, Expression<Func<TProjection, TField>> field, TField value, CancellationToken cancellationToken = default)
            => _collection.UpdateOneAsync(
                filter: filter,
                update: new ObjectUpdateDefinition<TProjection>(new()).Set(field, value),
                cancellationToken: cancellationToken);

        public Task UpdateOneFieldAsync<TField, TId>(TId id, Expression<Func<TProjection, TField>> field, TField value, CancellationToken cancellationToken = default) where TId : struct
            => _collection.UpdateOneAsync(
                filter: projection => projection.Id.Equals(id),
                update: new ObjectUpdateDefinition<TProjection>(new()).Set(field, value),
                cancellationToken: cancellationToken);

        public Task UpdateManyFieldAsync<TField>(Expression<Func<TProjection, bool>> filter, Expression<Func<TProjection, TField>> field, TField value, CancellationToken cancellationToken = default)
            => _collection.UpdateManyAsync(
                filter: filter,
                update: new ObjectUpdateDefinition<TProjection>(new()).Set(field, value),
                cancellationToken: cancellationToken);

        public ValueTask ReplaceInsertAsync(TProjection replacement, CancellationToken cancellationToken = default)
            => OnReplaceAsync(replacement, projection => projection.Id == replacement.Id, cancellationToken);

        public ValueTask ReplaceInsertAsync(TProjection replacement, Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken = default)
            => OnReplaceAsync(replacement, filter, cancellationToken);

        public ValueTask RebuildInsertAsync(TProjection replacement, CancellationToken cancellationToken = default)
            => OnReplaceAsync(replacement, projection => projection.Id == replacement.Id, cancellationToken);

        public ValueTask RebuildInsertAsync(TProjection replacement, Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken = default)
            => OnReplaceAsync(replacement, filter, cancellationToken);

        private async ValueTask OnReplaceAsync(TProjection replacement, Expression<Func<TProjection, bool>> filter, CancellationToken cancellationToken = default)
            => await _collection.ReplaceOneAsync(filter, replacement, new ReplaceOptions { IsUpsert = true }, cancellationToken);

        public IMongoCollection<TProjection> GetCollection()
                => _collection;
    }
}
