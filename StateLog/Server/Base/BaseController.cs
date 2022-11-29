namespace StateLog.Server;
[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<TEntity> : ControllerBase
    where TEntity : BaseEntity
{
    private readonly IBaseUnitOfWork<TEntity> _unitOfWork;
    public BaseController(IBaseUnitOfWork<TEntity> unitOfWork) => _unitOfWork = unitOfWork;

    [HttpPost]
    public virtual async Task Post(TEntity entity) => await _unitOfWork.Create(entity);

    [HttpGet]
    public virtual async Task<IEnumerable<TEntity>> Get() => await _unitOfWork.Read();
    [HttpGet("{id}")]
    public virtual async Task<TEntity> Get([FromRoute] Guid id) => await _unitOfWork.Read(id);

    [HttpPut]
    public async virtual Task Put(TEntity entity) => await _unitOfWork.Update(entity);

    [HttpDelete("{id}")]
    public async virtual Task Delete(Guid id) => await _unitOfWork.Delete(id);
}