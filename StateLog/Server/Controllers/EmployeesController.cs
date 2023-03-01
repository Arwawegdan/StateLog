namespace StateLog.Server;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private IEmployeeUnitOfWork _unitOfWork;
    public EmployeesController(IEmployeeUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public virtual async Task Post(Employee entity) => await _unitOfWork.Create(entity);

    [HttpGet("tagvalue/{tagValue}")]
    public virtual async Task<IEnumerable<Employee>> GetbyTagValue(string tagValue) => await _unitOfWork.ReadByTagValue(tagValue);

    [HttpGet("tagname/{tagName}")]
    public virtual async Task<IEnumerable<Employee>> GetByTagName(string tagName) => await _unitOfWork.ReadByTagName(tagName);

    [HttpGet]
    public virtual async Task<IEnumerable<Employee>> Get() => await _unitOfWork.Read();

    [HttpGet("{id}")]
    public virtual async Task<Employee> Get([FromRoute] Guid id) => await _unitOfWork.Read(id);

    [HttpPut]
    public async virtual Task Put(Employee entity) => await _unitOfWork.Update(entity);

    [HttpDelete]
    public async virtual Task Delete([FromBody] Employee entity) => await _unitOfWork.Delete(entity);

    [HttpDelete("{id}")]
    public async virtual Task Delete([FromRoute] Guid id) => await _unitOfWork.Delete(id);
}