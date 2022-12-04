namespace StateLog.Server;
public class CurrencyCosmosDbRepository : ICurrencyCosmosDbRepository 
{
    Container _container;
    private readonly CosmosClient dbClient;
    string Account = "https://statelog.documents.azure.com:443/";
    string Key = "pCy6KFciZqPs468Eq1k1ztCEInYMDIvMxKrDxsuGFafrlDUzzY9bYctkYqAcpBWOZqqUJAhMJaFJGcuiyvIaFA==";
    string databaseName = "ToDoList";
    string containerName = "Currency";
    Microsoft.Azure.Cosmos.Database database; 
    string partitionKey = "currency";   
 
    public CurrencyCosmosDbRepository()
    {
        CosmosClientOptions cosmosClientOptions = new CosmosClientOptions(){ApplicationName = "StateLog" };
        dbClient = new CosmosClient(Account, Key, cosmosClientOptions);
        CreateContainerAsync();
        _container = dbClient.GetContainer(databaseName, containerName);  
    }
    async Task CreateContainerAsync()
    {
        if(database == null)  database = await dbClient.CreateDatabaseIfNotExistsAsync(databaseName);
        _container = await database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");
    }

    public async Task<Currency> Add(Currency entity)
    {
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        await _container.CreateItemAsync<Currency>(entity, new PartitionKey(partitionKey));
        return entity;
    }
    public async Task<IEnumerable<Currency>> Add(IEnumerable<Currency> entities)
    {
        IEnumerable<Task<Currency>> creationTasks =
             entities.Select(value => Add(value)).ToList();
        await Task.WhenAll(creationTasks).ConfigureAwait(false);
        return (IEnumerable<Currency>)creationTasks;
    }

    public async Task<IEnumerable<Currency>> Get(string queryString)
    {
        FeedIterator<Currency> query = _container.GetItemQueryIterator<Currency>(new QueryDefinition(queryString));
        List<Currency> entities = new();
        while (query.HasMoreResults)
            entities.AddRange(await query.ReadNextAsync());
        return entities;
    }
    public async Task<Currency> Get(Guid id)
    {
        try
        {
            ItemResponse<Currency> itemResponse = await _container.ReadItemAsync<Currency>(id.ToString(), new PartitionKey(partitionKey));
            return itemResponse.Resource;
        }
        catch (CosmosException exception)
        {
            Log.Error(exception.Message);
            throw;
        }
    }
    public async Task<Currency> Update(Currency entity) 
                            => (await _container.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey))).Resource;
    
    public async Task<IEnumerable<Currency>> Update(IEnumerable<Currency> entities)
    {
        IEnumerable<Task<Currency>> UpdateTasks =
            entities.Select(value => Update(value)).ToList();
        await Task.WhenAll(UpdateTasks).ConfigureAwait(false);
        return (IEnumerable<Currency>)UpdateTasks;
    }
    public async Task<Currency> Delete(Currency TEntity) => (await _container.DeleteItemAsync<Currency>(TEntity.Id.ToString(), new PartitionKey(TEntity.PartitionKey))).Resource;

    public async Task<IEnumerable<Currency>> Delete(IEnumerable<Currency> entities)
    {
        IEnumerable<Task<Currency>> DeleteTasks =
           entities.Select(value => Delete(value)).ToList();
        await Task.WhenAll(DeleteTasks).ConfigureAwait(false);
        return (IEnumerable<Currency>)DeleteTasks;
    }

    public async Task<IEnumerable<Currency>> GetByTagValue(string id)
    {
        return await Get($"SELECT * FROM c where c.RowId like '%{id}'");
    }
    public async Task<IEnumerable<Currency>> GetByTagName(string id)
    {
        return await Get($"SELECT * FROM c where c.RowId like '%{id}'");
    }
}