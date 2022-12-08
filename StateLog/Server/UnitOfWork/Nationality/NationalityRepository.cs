namespace StateLog.Server;
public class NationalityRepository : BaseSettingsRepository<Nationality> , INationalityRepository
{
    public ApplicationDbContext Context { get; }
    public IReducerRepository _reducerRepository; 
    private readonly Queue<Nationality> nationalityQueue = new Queue<Nationality>(); 

    public NationalityRepository(ApplicationDbContext context, IReducerRepository reducerRepository) : base(context)
    {
        Context = context;
        _reducerRepository = reducerRepository;
    }
    //private readonly NationalityQueue nationalityQueue = new();
    public async Task Update(Nationality nationality)
    { 
        dbSet.Add(nationality);
        await Context.SaveChangesAsync();
        Reducer nationalityQueue = await QueueMapp(nationality);
        await _reducerRepository.Add(nationalityQueue);
    }

        public async Task Update(Nationality nationality , int operation = 0)
     {
        if (operation == 0)
        {
            await Update(nationality); 
        }
        else if (operation == 1)
        {
            nationality.NoOfEmployees += 1;
            await Update(nationality); 
        }   
        else if (operation == -1)
        {
            nationality.NoOfEmployees -= 1;
            await Update(nationality); 
        }
    }
    public async Task<Reducer> QueueMapp(Nationality nationality)
    {  
        Reducer reducer = new Reducer();
        reducer.Id = nationality.Id;
        reducer.BranchId = nationality.BranchId;
        reducer.ProductId = nationality.ProductId;
        reducer.PartitionKey = nationality.PartitionKey;
        reducer.TagValue = nationality.TagValue;
        reducer.TagName = nationality.TagName;
        reducer.Name = nationality.Name;
        reducer.CreatorId = nationality.CreatorId;
        reducer.Datetime = DateTime.Now;
        reducer.SchemaName = "Nationality"; 
        return reducer;
    }
    public async Task UpdateNationalities()
    {
        using IDbContextTransaction transaction = Context.Database.BeginTransaction();
        try
        {
            IEnumerable<Reducer> nationalityReducer = await _reducerRepository.Get();
            IEnumerable<Nationality> nationalitiesFtomDatabase = await dbSet.ToListAsync();
            nationalityReducer = nationalityReducer.OrderBy(e => e.Datetime).ToList();
            Nationality nationality = new Nationality();

            IList<Guid?> guids = nationalityReducer.Select(e => e.Id).Distinct().ToList();
            foreach (Guid? guid in guids)
            {
                foreach (Reducer nationalityReducerItem in nationalityReducer)
                {
                    if (guid == nationalityReducerItem.Id) 
                        nationality = MapFromNationalityReduerToNationality(nationalityReducerItem);
                }
                //guids.Remove(guid); 
                // no of requests = 2*(guids which have changed)  + etnen fo2 + 
                IEnumerable<Nationality> allIds = nationalitiesFtomDatabase.Where(e => e.Id == guid);
                // request per guid 
                dbSet.RemoveRange(allIds);
                dbSet.Add(nationality);
                await Context.SaveChangesAsync();
                transaction.Commit(); 
            }
        }
        catch (Exception exception)
        {
            Log.Error(exception.Message);
            transaction.Rollback();
            throw;
        }
    }
    public Nationality MapFromNationalityReduerToNationality(Reducer nationalityReducer)
    {
        Nationality nationality = new Nationality(); 
        nationality.ProductId = nationalityReducer.ProductId;
        nationality.PartitionKey = nationalityReducer.PartitionKey;
        nationality.BranchId = nationalityReducer.BranchId;
        nationality.CreatorId = nationalityReducer.CreatorId;
        nationality.BranchId = nationalityReducer.BranchId;
        nationality.Name = nationalityReducer.Name;
        nationality.TagName = nationalityReducer.TagName;
        nationality.TagValue = nationalityReducer.TagValue;
        nationality.Id = nationalityReducer.Id;
        return nationality; 
    }
}