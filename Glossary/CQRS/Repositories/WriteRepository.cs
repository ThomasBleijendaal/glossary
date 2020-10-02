using System;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Creations;
using CQRS.Operations;
using Microsoft.Azure.Cosmos.Table;

namespace CQRS.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, ITableEntity, new()
    {
        private readonly CloudTableClient _client;

        public WriteRepository(CloudStorageAccount cloudStorageAccount)
        {
            // weird DI resolving bug in azure functions..
            _client = (cloudStorageAccount ?? CloudStorageAccount.DevelopmentStorageAccount).CreateCloudTableClient();
        }

        public async Task CreateEntityAsync(ICreation<TEntity> creation)
        {
            var table = await GetTable();

            var entity = new TEntity
            {
                RowKey = new Random().Next(0, int.MaxValue).ToString(),
                PartitionKey = Program.PartitionKey
            };

            creation.Mutation.Invoke(entity);

            var operation = TableOperation.Insert(entity);

            await table.ExecuteAsync(operation);

            creation.CreatedId = int.Parse(entity.RowKey);
        }

        public async Task DeleteEntityAsync(int id)
        {
            var table = await GetTable();

            var entity = await GetByIdAsync(id, table);
            if (entity == null)
            {
                throw new NotFoundException();
            }

            var operation = TableOperation.Delete(entity);

            await table.ExecuteAsync(operation);
        }

        public async Task UpdateSingleEntityAsync(IOperation<TEntity> operation)
        {
            var table = await GetTable();

            var entity = table.CreateQuery<TEntity>().Where(operation.Criteria).FirstOrDefault();
            if (entity == null)
            {
                throw new NotFoundException();
            }

            operation.Mutation.Invoke(entity);

            if (operation.Validation.Invoke(entity))
            {
                var replace = TableOperation.Replace(entity);

                try
                {
                    var result = await table.ExecuteAsync(replace);
                }
                catch (StorageException)
                {
                    throw new ConflictException();
                }
            }
        }

        public async Task UpdateMultipleEntitiesAsync(IOperation<TEntity> operation)
        {
            var table = await GetTable();

            var entities = table.CreateQuery<TEntity>().Where(operation.Criteria).ToList();

            foreach (var entity in entities)
            {
                operation.Mutation.Invoke(entity);
            }

            var replaces = entities.Where(operation.Validation).Select(entity => TableOperation.Replace(entity));

            var batch = new TableBatchOperation();

            foreach (var replace in replaces)
            {
                batch.Append(replace);
            }

            // TODO: check operation for each entity
            var results = await table.ExecuteBatchAsync(batch);
        }

        private static async Task<TEntity> GetByIdAsync(int id, CloudTable table)
        {
            var retrieveOperation = TableOperation.Retrieve<TEntity>(Program.PartitionKey, id.ToString());

            var result = await table.ExecuteAsync(retrieveOperation);

            var entity = (TEntity)result.Result;
            return entity;
        }

        private async Task<CloudTable> GetTable()
        {
            var table = _client.GetTableReference(typeof(TEntity).Name);

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
