namespace NetCore.Data.Cosmos
{
    public class CosmosContainerSettings<T>
        where T : class
    {
        private string _endpoint = "https://handsoncrossplatform.documents.azure.com:443/";

        private string _key = "r5i23mty900v9mq7xgSp19MZZTbLP2A5VI9YkxyCkenGzCjHvSrFOml5JFJo6VAzpQ9TELnpE6BpXrKudHMeVg==";

        public CosmosContainerSettings(string collectionId)
        {
            CollectionId = collectionId;
        }

        public string DatabaseId1 { set; get; } = "ProductsDb";

        public string CollectionId { set; get; } = "";

        public string Endpoint
        {
            get
            {
                return _endpoint;
            }
            set
            {
                _endpoint = value;
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
    }
}