using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;

using FluentAssertions;

using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

using Newtonsoft.Json;

using Xunit;

namespace FirstXamarinFormsApplication.Client.Tests
{
    public class ClientIntegrationTests : IDisposable
    {
        private DataIntegrationFixture _fixture = new DataIntegrationFixture();

        private IRepository<Product,string> _repository = new CosmosProductsRepository();

        private Task _initialized;

public ClientIntegrationTests()
{
    _fixture.Register<IRepository<Product, string>>(() => _repository);

    var products = _fixture.Build<Product>().With(item => item.Id, string.Empty).CreateMany(9);

   _fixture.RegisterProducts(products).Wait();
}

[Fact(DisplayName = "Api Client Should Retrieve All Products")]
[Trait("Category", "Integration")]
public async Task ApiClient_GetProducts_RetrieveAll()
{
    #region Arrange

    var expectedCollection = _fixture.Create<IEnumerable<Product>>();

    #endregion

    #region Act

    var apiClient = new ApiClient();

    var actualResultSet = await apiClient.RetrieveProductsAsync();

    #endregion

    #region Assert

    actualResultSet.Should().HaveCount(expectedCollection.Count());

    #endregion
}

public void Dispose()
{
    _fixture.Reset().Wait();
}
    }

public class DataIntegrationFixture : Fixture
{
    public async Task RegisterProducts(IEnumerable<Product> products)
    {
        var dbRepository = this.Create<IRepository<Product, string>>();

        foreach (var product in products)
        {
            await dbRepository.AddItemAsync(product);
        }

        this.Register(() => products);
    }

    public async Task Reset()
    {
        var dbRepository = this.Create<IRepository<Product, string>>();

        var items = this.Create<IEnumerable<Product>>();

        foreach (var product in items)
        {
            await dbRepository.DeleteItemAsync(product.Id);
        }
    }
}

    /// <summary>
    /// The interface contains methods for Operations on Collections
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public interface IRepository<TModel, in TPrimaryKey>
    {
        Task<IEnumerable<TModel>> GetItemsAsync();
        Task<TModel> GetItemAsync(TPrimaryKey id);
        Task<TModel> AddItemAsync(TModel item);
        Task<TModel> UpdateItemAsync(TPrimaryKey id, TModel item);
        Task DeleteItemAsync(TPrimaryKey id);
    }

    /// <summary>
    /// The following class is used to create DocumentDB, Collections if not exist
    /// </summary>
    public class CosmosProductsRepository : IRepository<Product, string>
    {
        #region The DocumentDB Endpoint, Key, DatabaseId and CollectionId declaration
        private static readonly string Endpoint = "https://handsoncrossplatform.documents.azure.com:443/";
        private static readonly string Key = "r5i23mty900v9mq7xgSp19MZZTbLP2A5VI9YkxyCkenGzCjHvSrFOml5JFJo6VAzpQ9TELnpE6BpXrKudHMeVg==";
        private static readonly string DatabaseId = "ProductsDb";
        private static readonly string CollectionId = "ProductsCollection";
        private static DocumentClient docClient;
        #endregion

        public CosmosProductsRepository()
        {
            docClient = new DocumentClient(new Uri(Endpoint), Key);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        #region Private methods to create Database and Collection if not Exist
        /// <summary>
        /// The following function has following steps
        /// 1. Try to read database based on the DatabaseId passed as URI link, if it is not found the execption will be thrown
        /// 2. In the execption the database will be created of which Id will be set as DatabaseId 
        /// </summary>
        /// <returns></returns>
        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                //1.
                await docClient.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2.
                    await docClient.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// The following function has following steps
        /// 1.Read the collection based on the DatabaseId and Collectionid passed as URI, if not found then throw exeption
        /// //2.In exception create a collection.
        /// </summary>
        /// <returns></returns>
        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                //1.
                await docClient.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    //2.
                    await docClient.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId},
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// The method to create a new Document in the collection 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Product> AddItemAsync(Product item)
        {
            try
            {
                var document = await docClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
                var res = document.Resource;
                var person = JsonConvert.DeserializeObject<Product>(res.ToString());
                item.Id = person.Id;
                return person;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Delete document
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteItemAsync(string id)
        {
            await docClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        /// <summary>
        /// Method to read Item from the document based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Product> GetItemAsync(string id)
        {
            try
            {
                Document doc = await docClient.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return JsonConvert.DeserializeObject<Product>(doc.ToString());
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


        /// <summary>
        /// Method to Read all Documents from the collection
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Product>> GetItemsAsync()
        {
            var documents = docClient.CreateDocumentQuery<Product>(
                  UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                  new FeedOptions { MaxItemCount = -1 })
                  .AsDocumentQuery();

            List<Product> persons = new List<Product>();

            while (documents.HasMoreResults)
            {
                persons.AddRange(await documents.ExecuteNextAsync<Product>());
            }

            return persons;
        }

        /// <summary>
        /// Method to Update Document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<Product> UpdateItemAsync(string id, Product item)
        {
            try
            {
                var document = await docClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
                var data = document.Resource.ToString();

                var product = JsonConvert.DeserializeObject<Product>(data);
                return product;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
