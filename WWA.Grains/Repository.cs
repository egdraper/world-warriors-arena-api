using MongoDB.Bson;
using MongoDB.Driver;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WWA.GrainInterfaces.Models;
using WWA.Grains.Entities;
using WWA.Grains.Mongo;

namespace WWA.Grains
{
    public class Repository<TEntity> : Grain<TEntity> where TEntity : Entity
    {
        private string _collectionName { get; set; }
        private IMongoCollection<TEntity> _collection { get; set; }

        protected FilterDefinitionBuilder<TEntity> _filter => Builders<TEntity>.Filter;
        protected IndexKeysDefinitionBuilder<TEntity> _index => Builders<TEntity>.IndexKeys;
        protected SortDefinitionBuilder<TEntity> _sort => Builders<TEntity>.Sort;
        protected ProjectionDefinitionBuilder<TEntity> _projection => Builders<TEntity>.Projection;

        public Repository(IMongoContext mongoContext, string collectionName)
        {
            _collectionName = collectionName;
            _collection = mongoContext.Database.GetCollection<TEntity>(_collectionName);
        }

        #region Service Methods
        public virtual Task<int> QueryAsync()
        {
            return QueryAsync(_filter.Empty);
        }

        public virtual Task<TEntity> GetAsync()
        {
            return GetAsync(_filter.Empty);
        }

        public virtual Task<List<TEntity>> ListAsync()
        {
            return ListAsync(_filter.Empty);
        }

        public virtual Task<PaginatedEntityModel<TEntity>> ListPagedAsync()
        {
            return ListPagedAsync(_filter.Empty);
        }

        public virtual Task CreateAsync()
        {
            return CreateAsync();
        }

        public virtual Task<List<TProjection>> ProjectAsync<TProjection>()
        {
            return ProjectAsync<TProjection>();
        }

        public virtual Task<List<TAggregate>> AggregateAsync<TAggregate>()
        {
            return AggregateAsync<TAggregate>();
        }

        public virtual Task<List<TAggregate>> AggregatePagedAsync<TAggregate>()
        {
            return AggregatePagedAsync<TAggregate>();
        }

        public virtual Task DeleteAsync()
        {
            return DeleteAsync();
        }
        #endregion

        #region Repository Methods
        protected async Task<int> QueryAsync(FilterDefinition<TEntity> filterDefinition)
        {
            var count = await _collection.CountDocumentsAsync(filterDefinition);
            return (int)count;
        }

        protected async Task<TEntity> GetAsync(FilterDefinition<TEntity> filterDefinition)
        {
            var entity = await _collection.FindAsync(filterDefinition);
            return entity.SingleOrDefault();
        }

        protected async Task<List<TEntity>> ListAsync(
            FilterDefinition<TEntity> filterDefinition,
            SortDefinition<TEntity> sortDefinition = null)
        {
            var list = await _collection.FindAsync(
                filterDefinition,
                new FindOptions<TEntity>
                {
                    Sort = sortDefinition
                });
            return list.ToList();
        }

        protected async Task<PaginatedEntityModel<TEntity>> ListPagedAsync(
            FilterDefinition<TEntity> filterDefinition,
            SortDefinition<TEntity> sortDefinition = null,
            int? skip = null,
            int? take = null)
        {
            var count = await _collection.CountDocumentsAsync(filterDefinition);
            var paged = await _collection.FindAsync(
                filterDefinition,
                new FindOptions<TEntity, TEntity>
                {
                    Sort = sortDefinition,
                    Skip = skip,
                    Limit = take
                });
            var page = new PaginatedEntityModel<TEntity>
            {
                TotalCount = (int)count,
                Page = paged.ToList()
            };
            return page;
        }

        protected async Task CreateAsync(TEntity entity)
        {
            if (entity is TrackedEntity tracked)
            {
                tracked.DateCreated = DateTime.UtcNow;
                tracked.DateModified = DateTime.UtcNow;
            }
            await _collection.InsertOneAsync(entity);
        }

        protected async Task<List<TProjection>> ProjectAsync<TProjection>(
            FilterDefinition<TEntity> filterDefinition,
            ProjectionDefinition<TEntity, TProjection> projectionDefinition)
        {
            var projection = await _collection.FindAsync(
                filterDefinition,
                new FindOptions<TEntity, TProjection>
                {
                    Projection = projectionDefinition
                });
            return projection.ToList();
        }

        protected async Task<List<TAggregate>> AggregateAsync<TAggregate>(
            PipelineDefinition<TEntity, TAggregate> pipelineDefinition,
            AggregateOptions aggregateOptions)
        {
            var aggregate = await _collection.AggregateAsync(pipelineDefinition, aggregateOptions);
            return aggregate.ToList();
        }

        protected async Task<PaginatedEntityModel<TAggregate>> AggregatePagedAsync<TAggregate>(
            PipelineDefinition<TEntity, TAggregate> pipelineDefinition,
            AggregateOptions aggregateOptions,
            int? skip = null,
            int? take = null)
        {
            var aggregate = await _collection.AggregateAsync(pipelineDefinition, aggregateOptions);
            var list = aggregate.ToList();
            var count = list.Count;
            var paged = list.GetRange(skip ?? 0, take ?? list.Count);
            var page = new PaginatedEntityModel<TAggregate>
            {
                TotalCount = count,
                Page = paged
            };
            return page;
        }

        protected async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(_filter.Eq("_id", ObjectId.Parse(id)));
            return;
        }
        #endregion


        // Create Index Methods

        protected Task CreateIndexesAsync(IndexKeysDefinition<TEntity> index)
        {
            return _collection.Indexes.CreateOneAsync(new CreateIndexModel<TEntity>(index));
        }
        protected Task CreateUniqueIndexAsync(IndexKeysDefinition<TEntity> index)
        {
            var createIndexModel = new CreateIndexModel<TEntity>(index, new() { Unique = true });
            return _collection.Indexes.CreateOneAsync(createIndexModel);
        }

        // Ensure Indices
        public virtual Task CreateIndicesAsync() => Task.CompletedTask;
    }
}
