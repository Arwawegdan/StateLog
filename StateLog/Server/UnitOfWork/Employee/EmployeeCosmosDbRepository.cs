namespace StateLog.Server;
public class EmployeeCosmosDbRepository : IEmployeeCosmosDbRepository
{
    Container _container;
    private readonly CosmosClient dbClient;
    string Account = "https://statelog.documents.azure.com:443/";
    string Key = "pCy6KFciZqPs468Eq1k1ztCEInYMDIvMxKrDxsuGFafrlDUzzY9bYctkYqAcpBWOZqqUJAhMJaFJGcuiyvIaFA==";
    string databaseName = "ToDoList";
    string containerName = "Nationality";
    Microsoft.Azure.Cosmos.Database database; 
    string partitionKey = "employee";   
 
    public EmployeeCosmosDbRepository()
    {
        CosmosClientOptions cosmosClientOptions = new CosmosClientOptions(){ApplicationName = "StateLog" , AllowBulkExecution = true };
        dbClient = new CosmosClient(Account, Key, cosmosClientOptions);
        CreateContainerAsync();
        _container =  dbClient.GetContainer(databaseName, containerName);  
    }
    async Task CreateContainerAsync()
    {
        while(database == null) database = await dbClient.CreateDatabaseIfNotExistsAsync(databaseName);
        _container = await  database.CreateContainerIfNotExistsAsync(containerName, "/partitionKey");
    }

    public async Task<Employee> Add(Employee entity)
    {
        if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
        await _container.CreateItemAsync<Employee>(entity, new PartitionKey("employee"));
        return entity;
    }
    public async Task<IEnumerable<Employee>> Add(IEnumerable<Employee> entities)
    {
        IEnumerable<Task<Employee>> creationTasks =
             entities.Select(value => Add(value)).ToList();
        await Task.WhenAll(creationTasks).ConfigureAwait(false);
        return (IEnumerable<Employee>)creationTasks;
    }

    public async Task<IEnumerable<Employee>> Get(string queryString)
    {
        queryString = $"select * from c where c.partitionKey like  '%{partitionKey}'";
        FeedIterator<Employee> query = _container.GetItemQueryIterator<Employee>(new QueryDefinition(queryString));
        List<Employee> entities = new();
        while (query.HasMoreResults)
            entities.AddRange(await query.ReadNextAsync());
        return entities;
    }
    public async Task<Employee> Get(Guid id)
    {
        try
        {
            ItemResponse<Employee> itemResponse = await _container.ReadItemAsync<Employee>(id.ToString(), new PartitionKey(partitionKey));
            return itemResponse.Resource;
        }
        catch (CosmosException exception)
        {
            Log.Error(exception.Message);
            throw;
        }
    }
    public async Task<Employee> Update(Employee entity) 
                            => (await _container.UpsertItemAsync(entity, new PartitionKey(entity.PartitionKey))).Resource;
    
    public async Task<IEnumerable<Employee>> Update(IEnumerable<Employee> entities)
    {
        IEnumerable<Task<Employee>> UpdateTasks =
            entities.Select(value => Update(value)).ToList();
        await Task.WhenAll(UpdateTasks).ConfigureAwait(false);
        return (IEnumerable<Employee>)UpdateTasks;
    }
    public async Task<Employee> Delete(Employee TEntity) => (await _container.DeleteItemAsync<Employee>(TEntity.Id.ToString(), new PartitionKey(TEntity.PartitionKey))).Resource;

    public async Task<IEnumerable<Employee>> Delete(IEnumerable<Employee> entities)
    {
        IEnumerable<Task<Employee>> DeleteTasks =
           entities.Select(value => Delete(value)).ToList();
        await Task.WhenAll(DeleteTasks).ConfigureAwait(false);
        return (IEnumerable<Employee>)DeleteTasks;
    }

    public async Task<IEnumerable<Employee>> GetByTagValue(string id)
    {
        return await Get($"SELECT * FROM c where c.RowId like '%{id}'");
    }
    public async Task<IEnumerable<Employee>> GetByTagName(string id)
    {
        return await Get($"SELECT * FROM c where c.RowId like '%{id}'");
    }
}