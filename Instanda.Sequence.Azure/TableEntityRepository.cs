using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Instanda.Sequence.Azure
{
    public abstract class TableEntityRepository<T> where T : ITableEntity, new()
    {


        public bool ThrowExceptions { get; set; }

        protected readonly Lazy<CloudTable> CloudTable;

        protected TableEntityRepository(string connectionStr, string tableName, bool throwExceptions)
        {
            if (connectionStr == null) throw new ArgumentNullException("connectionStr");
            if (tableName == null) throw new ArgumentNullException("tableName");

            if (!Regex.IsMatch(tableName, "^[A-Za-z][A-Za-z0-9]{2,62}$"))
            {
                throw new AzureTableNameException("{0} is not valid table name", tableName);
            }

            ThrowExceptions = throwExceptions;

            CloudTable = new Lazy<CloudTable>(
            () =>
            {
                var storageAccount = CloudStorageAccount.Parse(connectionStr);

                var tableClient = storageAccount.CreateCloudTableClient();

                var table = tableClient.GetTableReference(tableName);

                table.CreateIfNotExists();

                return table;
            }
            );
        }

        /// <exception cref="AzureTableLookUpException" />
        public async Task<T> Get(string partitionKey, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            var retrievedResult = await CloudTable.Value.ExecuteAsync(retrieveOperation);

            T savedEntity;
            if (TryGetResult(retrievedResult, out savedEntity))
            {
                return savedEntity;
            }
            if (ThrowExceptions)
            {
                throw new AzureTableLookUpException(string.Format("partition key:{0} row key: {1}", partitionKey, rowKey), retrievedResult.HttpStatusCode);
            }
            return savedEntity;
        }

        /// <exception cref="AzureTableLookUpException" />
        public async Task<IEnumerable<T>> ListByRowKey(string rowKey)
        {
            var retrieveOperation = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToLower()));

            var retrievedResult = await ExecuteQueryAsync(retrieveOperation);

            IEnumerable<T> results = null;
            try
            {
                results = retrievedResult.Results;
            }
            catch (StorageException ex)
            {
                if (ThrowExceptions)
                {
                    throw new AzureTableLookUpException(string.Format("rowKey key:{0}", rowKey),
                        ex.RequestInformation.HttpStatusCode);
                }
            }

            return results;
        }

        /// <exception cref="AzureTableLookUpException" />
        public async Task<IEnumerable<T>> List(string partitionKey)
        {
            var retrieveOperation = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey.ToLower()));

            var retrievedResult = await ExecuteQueryAsync(retrieveOperation);

            IEnumerable<T> results = null;
            try
            {
                results = retrievedResult.Results;
            }
            catch (StorageException ex)
            {
                if (ThrowExceptions)
                {
                    throw new AzureTableLookUpException(string.Format("partition key:{0}", partitionKey),
                        ex.RequestInformation.HttpStatusCode);
                }
            }

            return results;
        }

        protected class ListResult<TResult>
        {
            public IEnumerable<TResult> Results { get; set; }
            public TableContinuationToken ContinuationToken { get; set; }
        }


        internal Task<TableQuerySegment<T>> ExecuteQueryAsync(
            TableQuery<T> query,
            TableContinuationToken token = default(TableContinuationToken),
            CancellationToken ct = default(CancellationToken))
        {
            token = token ?? new TableContinuationToken();

            ICancellableAsyncResult ar = CloudTable.Value.BeginExecuteQuerySegmented(query, token, null, null);
            ct.Register(ar.Cancel);

            return Task.Factory.FromAsync<TableQuerySegment<T>>(ar, CloudTable.Value.EndExecuteQuerySegmented<T>);
        }



        /// <exception cref="AzureTableAddException" />
        public async Task<T> Add(T entity)
        {

            var insertOperation = TableOperation.Insert(entity);

            return await Add(insertOperation);

        }

        /// <exception cref="AzureTableAddException" />
        public async Task<T> AddOrReplace(T entity)
        {

            var insertOperation = TableOperation.InsertOrReplace(entity);

            return await Add(insertOperation);

        }


        private async Task<T> Add(TableOperation insertOperation)
        {

            try
            {
                var retrievedResult = await CloudTable.Value.ExecuteAsync(insertOperation);

                T savedEntity;
                if (TryGetResult(retrievedResult, out savedEntity))
                {
                    return savedEntity;
                }

                if (ThrowExceptions)
                {
                    throw new AzureTableAddException(string.Format("Failed to add to Azure table storage"), retrievedResult.HttpStatusCode);
                }
                return savedEntity;
            }
            catch (StorageException ex)
            {

                if (ThrowExceptions)
                {
                    throw new AzureTableAddException(ex.Message, ex.RequestInformation.HttpStatusCode);
                }

                return default(T);
            }

        }

        /// <exception cref="AzureTableUpdateException" />
        public async Task<T> UpdateAsync(T entity)
        {

            var updateOperation = TableOperation.Replace(entity);

            try
            {
                // Execute the insert operation.
                var retrievedResult = await CloudTable.Value.ExecuteAsync(updateOperation);

                T savedEntity;
                if (TryGetResult(retrievedResult, out savedEntity))
                {
                    return savedEntity;
                }

                if (ThrowExceptions)
                {
                    throw new AzureTableUpdateException(string.Format("Failed to update to Azure table storage"), retrievedResult.HttpStatusCode);
                }

                return savedEntity;

            }
            catch (StorageException ex)
            {

                if (ThrowExceptions)
                {
                    throw new AzureTableUpdateException(ex.Message, ex.RequestInformation.HttpStatusCode);
                }

                return default(T);
            }

        }



        /// <exception cref="AzureTableDeleteException" />
        public async Task<T> Delete(T entity)
        {

            var operation = TableOperation.Delete(entity);

            // Execute the insert operation.
            var retrievedResult = await CloudTable.Value.ExecuteAsync(operation);

            T savedEntity;
            if (TryGetResult(retrievedResult, out savedEntity))
            {
                return savedEntity;
            }
            if (ThrowExceptions)
            {
                throw new AzureTableDeleteException(string.Format("Failed to delete from Azure table storage"), retrievedResult.HttpStatusCode);
            }
            return savedEntity;


        }

        private static bool TryGetResult(TableResult retrievedResult, out T entity)
        {
            if (retrievedResult.HttpStatusCode == 200 || retrievedResult.HttpStatusCode == 204 || retrievedResult.HttpStatusCode == 404)
            {
                entity = (T)retrievedResult.Result;
                return true;
            }
            entity = default(T);
            return false;
        }


    }
}
