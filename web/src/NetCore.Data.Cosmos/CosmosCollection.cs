using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

using NetCore.Web.Data.Abstractions;
using User = NetCore.Data.User;

namespace NetCore.Data.Cosmos
{
    public class CosmosCollection<T> : IRepository<T> where T : class
    {
        private DocumentClient _client;

        private readonly CosmosContainerSettings<T> _cosmosContainerSettings;

        public CosmosCollection(string collectionName)
        {
            _cosmosContainerSettings = new CosmosContainerSettings<T>(collectionName);

            _client = new DocumentClient(new Uri(_cosmosContainerSettings.Endpoint), _cosmosContainerSettings.Key);
        }

        public async Task InsertAuction(string userId, Auction auction)
        {
            try
            {
                RequestOptions options = new RequestOptions { PartitionKey = new PartitionKey("USA") };
                //options.PreTriggerInclude = new[] { "updateAggregates" };

                var spLink = UriFactory.CreateStoredProcedureUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId, "insertAuction");

                var result = await _client.ExecuteStoredProcedureAsync<User>(spLink, null, userId, (dynamic)auction);
            }
            catch (DocumentClientException e)
            {
                throw;
            }
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                var requestOption = new RequestOptions();
                requestOption.PartitionKey = new PartitionKey("USA");

                Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId, id), requestOption);
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public IQueryable<T> GetItemsAsync()
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1, PopulateQueryMetrics = true, EnableCrossPartitionQuery = true };

            IOrderedQueryable<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId),
                feedOptions);

            return query;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, string partitionKey = null)
        {
            var feedOptions = new FeedOptions { MaxItemCount = -1, PopulateQueryMetrics = true };

            if (string.IsNullOrEmpty(partitionKey))
            {
                feedOptions.EnableCrossPartitionQuery = true;
                System.Diagnostics.Debug.WriteLine("Executing Query without PartitionKey");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Executing Query with PartitionKey");
                feedOptions.PartitionKey = new PartitionKey(partitionKey);
            }

            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
            UriFactory.CreateDocumentCollectionUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId), feedOptions)
                .Where(predicate)
                .AsDocumentQuery();


            System.Diagnostics.Debug.WriteLine($"Query: {query.ToString()}");

            List<T> results = new List<T>();

            while (query.HasMoreResults)
            {
                var response = await query.ExecuteNextAsync<T>();
                System.Diagnostics.Debug.WriteLine($"{response.RequestCharge}RU");
                System.Diagnostics.Debug.WriteLine("QueryMetrics:");
                System.Diagnostics.Debug.WriteLine(response.QueryMetrics.FirstOrDefault());
                results.AddRange(response);
            }

            return results;
        }

        public async Task<T> AddItemAsync(T item)
        {
            return (T)(dynamic)await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId), item);
        }

        public async Task<T> UpdateItemAsync(string id, T item)
        {
            var requestOption = new RequestOptions();
            //requestOption.PreTriggerInclude = new[] { "updateAggregate" };
            return (T)(dynamic)(await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId, id), item, requestOption)).Resource;
        }

        public async Task DeleteItemAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_cosmosContainerSettings.DatabaseId1, _cosmosContainerSettings.CollectionId, id));
        }
    }
}
