namespace StateLog.Server;
public class NationalityRepository : BaseSettingsRepository<Nationality> , INationalityRepository
{
    public ApplicationDbContext Context { get; }
    public INationalityReducerRepository _nationalityReducerRepository; 
    private readonly Queue<Nationality> nationalityQueue = new Queue<Nationality>(); 

    public NationalityRepository(ApplicationDbContext context, INationalityReducerRepository nationalityReducerRepository) : base(context)
    {
        Context = context;
        _nationalityReducerRepository = nationalityReducerRepository;
    }

    //private readonly NationalityQueue nationalityQueue = new();   

    public async Task Update(Nationality nationality)
    {
        
       // nationality.Id = Guid.NewGuid(); 
        dbSet.Add(nationality);
        await Context.SaveChangesAsync();

        NationalityReducer nationalityQueue = await QueueMapp(nationality); 
        await _nationalityReducerRepository.Add(nationalityQueue);

        // await Task.Run(()=> Context.Update(nationality)); 
        //nationalityQueue.Id = Guid.NewGuid();
        //nationalityQueue.NationalityId = nationality.Id; 
        //nationalityQueue.Nationality = nationality; 
        //nationalityQueue.Queue.Append(nationality);
        //NationalityQueue nationalityQueue = await QueueMapp(nationality); 
        //await _nationalityQueueRepository.Add(nationalityQueue);
        //await Context.SaveChangesAsync();
        //dbSet.AddAsync(nationality);
        //await Context.SaveChangesAsync();
    }
     
    public async Task<NationalityReducer> QueueMapp(Nationality nationality)
    {
        //NationalityQueue nationalityQueue = new();
        //await Task.Run(()=>nationalityQueue.Enqueue(nationality)); 
        //nationalityQueue.Id = new Guid();
        //return nationalityQueue;    
        NationalityReducer nationalityQueue = new NationalityReducer();
        nationalityQueue.Id = nationality.Id;
        nationalityQueue.BranchId = nationality.BranchId;
        nationalityQueue.ProductId = nationality.ProductId;
        nationalityQueue.PartitionKey = nationality.PartitionKey;
        nationalityQueue.TagValue = nationality.TagValue;
        nationalityQueue.TagName = nationality.TagName;
        nationalityQueue.Name = nationality.Name;
        nationalityQueue.CreatorId = nationalityQueue.CreatorId;
        nationalityQueue.Datetime = DateTime.Now; 
        return nationalityQueue;
    }
    public  async Task UpdateNationalities()
    {
        using IDbContextTransaction transaction = Context.Database.BeginTransaction();
        try
        {
            IEnumerable<NationalityReducer> nationalityReducer = await _nationalityReducerRepository.Get();
            IEnumerable<Nationality> nationalitiesFtomDatabase = await dbSet.ToListAsync();
            nationalityReducer = nationalityReducer.OrderBy(e => e.Datetime).ToList();
            Nationality nationality = new Nationality();

            IList<Guid?> guids = nationalityReducer.Select(e => e.Id).Distinct().ToList();
            foreach (Guid? guid in guids)
            {
                foreach (NationalityReducer nationalityReducerItem in nationalityReducer)
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
    public Nationality MapFromNationalityReduerToNationality(NationalityReducer nationalityReducer)
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