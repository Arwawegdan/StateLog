namespace StateLog.Server;
[Route("api/[controller]")]
[ApiController]
public class CurrenciesController : ControllerBase
{
    private ICurrencyUnitOfWork _unitOfWork;
    public CurrenciesController(ICurrencyUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpPost]
    public virtual async Task Post(Currency entity) => await _unitOfWork.Create(entity);

    [HttpGet("tagvalue/{tagValue}")]
    public virtual async Task<IEnumerable<Currency>> GetbyTagValue(string tagValue) => await _unitOfWork.ReadByTagValue(tagValue);

    [HttpGet("tagname/{tagName}")]
    public virtual async Task<IEnumerable<Currency>> GetByTagName(string tagName) => await _unitOfWork.ReadByTagName(tagName);

    [HttpGet]
    public virtual async Task<IEnumerable<Currency>> Get() => await _unitOfWork.Read();

    [HttpGet("{id}")]
    public virtual async Task<Currency> Get([FromRoute] Guid id) => await _unitOfWork.Read(id);

    [HttpPut]
    public async virtual Task Put(Currency entity) => await _unitOfWork.Update(entity);

    [HttpDelete]
    public async virtual Task Delete([FromBody] Currency entity) => await _unitOfWork.Delete(entity);

    [HttpDelete("{id}")]
    public async virtual Task Delete([FromRoute] Guid id) => await _unitOfWork.Delete(id);
}