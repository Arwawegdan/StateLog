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
    public async Task UpdateNationalitie()
    {
        IEnumerable<NationalityReducer> nationalityReducer  = await _nationalityReducerRepository.Get();

        Dictionary<Guid?, DateTime> reducer =  new Dictionary<Guid?, DateTime>();

        foreach (NationalityReducer nationalityReducerItem in nationalityReducer)
        {
            reducer.Add(nationalityReducerItem.Id, nationalityReducerItem.Datetime); 
        }
        Dictionary<Guid?, DateTime> FinalReducer = new Dictionary<Guid?, DateTime>();      
    }
}