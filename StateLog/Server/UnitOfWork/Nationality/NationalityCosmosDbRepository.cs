namespace StateLog.Server;
public class NationalityCosmosDbRepository : INationalityCosmosDbRepository 
{
    Container _container;
    CosmosClient dbClient;
    string Account = "https://statelog.documents.azure.com:443/";
    string Key = "pCy6KFciZqPs468Eq1k1ztCEInYMDIvMxKrDxsuGFafrlDUzzY9bYctkYqAcpBWOZqqUJAhMJaFJGcuiyvIaFA==";
    string databaseName = "ToDoList";
    string containerName = "Nationality";
    Microsoft.Azure.Cosmos.Database database = null;
    string partitionKey = "nationality";   
 
    public NationalityCosmosDbRepository()
    {
        CosmosClientOptions cosmosClientOptions = new CosmosClientOptions(){ApplicationName = "StateLog"};
        dbClient = new CosmosClient(Account, Key, cosmosClientOptions);
        CreateContainerAsync();
        _container = dbClient.GetContainer(databaseName, containerName);  
    }
    async Task CreateContainerAsync()
    {
        if(database == null)  database = await dbClient.CreateDatabaseIfNotExistsAsync(databaseName);
        _container = await database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");
    }

    public async Task<Nationality> Add(Nationality entity)
    {
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        await _container.CreateItemAsync<Nationality>(entity, new PartitionKey(partitionKey));
        return entity;
    }
    public async Task<IEnumerable<Nationality>> Add(IEnumerable<Nationality> entities)
    {
        IEnumerable<Task<Nationality>> creationTasks =
            entities.Select(value => Add(value)).ToList();
        await Task.WhenAll(creationTasks).ConfigureAwait(false);
        return (IEnumerable<Nationality>)creationTasks;
    }

    public async Task<IEnumerable<Nationality>> Get(string queryString)
    {

        queryString = $"select * from c where c.partitionKey like '%{partitionKey}'"; 
        FeedIterator<Nationality> query = _container.GetItemQueryIterator<Nationality>(new QueryDefinition(queryString));
        List<Nationality> entities = new();
        while (query.HasMoreResults)
            entities.AddRange(await query.ReadNextAsync());
        return entities;
    }
    public async Task<Nationality> Get(Guid id)
    {
        try
        {
            ItemResponse<Nationality> itemResponse = await _container.ReadItemAsync<Nationality>(id.ToString(), new PartitionKey(partitionKey));
            return itemResponse.Resource;
        }
        catch (CosmosException exception)
        {
            Log.Error(exception.Message);
            throw;
        }
    }
    public async Task<Nationality> Update(Nationality entity) 
                            => (await _container.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey))).Resource;
    
    public async Task<IEnumerable<Nationality>> Update(IEnumerable<Nationality> entities)
    {
        IEnumerable<Task<Nationality>> UpdateTasks =
            entities.Select(value => Update(value)).ToList();
        await Task.WhenAll(UpdateTasks).ConfigureAwait(false);
        return (IEnumerable<Nationality>)UpdateTasks;
    }
    public async Task<Nationality> Delete(Nationality TEntity)
        => (await _container.DeleteItemAsync<Nationality>(TEntity.Id.ToString(), new PartitionKey(TEntity.PartitionKey))).Resource;

    public async Task<IEnumerable<Nationality>> Delete(IEnumerable<Nationality> entities)
    {
        IEnumerable<Task<Nationality>> DeleteTasks =
           entities.Select(value => Delete(value)).ToList();
        await Task.WhenAll(DeleteTasks).ConfigureAwait(false);
        return (IEnumerable<Nationality>)DeleteTasks;
    }

    public async Task<IEnumerable<Nationality>> GetByTagValue(string id)         
                  => await Get($"SELECT * FROM c where c.RowId like '%{id}'");
   
    public async Task<IEnumerable<Nationality>> GetByTagName(string id) 
                 => await Get($"SELECT * FROM c where c.RowId like '%{id}'");
}