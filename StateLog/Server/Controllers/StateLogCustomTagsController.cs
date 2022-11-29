namespace StateLog.Server;
[Route("api/[controller]")]
[ApiController]
public class StateLogCustomTagsController : BaseController<StateLogCustomTags>
{
    private IStateLogCustomTagsUnitOfWork _unitOfWork;
    public StateLogCustomTagsController(IStateLogCustomTagsUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}