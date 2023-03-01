namespace StateLog.Server;
[Route("api/[controller]")]
[ApiController]
public class NationalitiesController : ControllerBase
{
    private INationalityUnitOfWork _unitOfWork;
    public NationalitiesController(INationalityUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpPost]
    public virtual async Task Post(Nationality entity) => await _unitOfWork.Create(entity);

    [HttpGet("tagvalue/{tagValue}")]
    public virtual async Task<IEnumerable<Nationality>> GetbyTagValue(string tagValue) => await _unitOfWork.ReadByTagValue(tagValue);

    [HttpGet("tagname/{tagName}")]
    public virtual async Task<IEnumerable<Nationality>> GetByTagName(string tagName) => await _unitOfWork.ReadByTagName(tagName);

    [HttpGet]
    public virtual async Task<IEnumerable<Nationality>> Get() => await _unitOfWork.Read();

    [HttpGet("{id}")]
    public virtual async Task<Nationality> Get([FromRoute] Guid id) => await _unitOfWork.Read(id);

    [HttpPut]
    public async virtual Task Put(Nationality entity) => await _unitOfWork.Update(entity);

    [HttpPut("updatelastvalue")]
    public async virtual Task Put() => await _unitOfWork.NationalityReducer();

    [HttpDelete("{id}")]
    public async virtual Task Delete([FromRoute] Guid id) => await _unitOfWork.Delete(id);

    [HttpDelete]
    public async virtual Task Delete([FromBody] Nationality entity) => await _unitOfWork.Delete(entity);
}