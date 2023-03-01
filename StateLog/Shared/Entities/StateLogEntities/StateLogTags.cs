namespace StateLog.Shared;
public class StateLogTags : BaseEntity
{
    public Guid? RowId { get; set; }    
    
    public Guid? ProductId { get; set; }
   
    public Guid? CompanyId { get; set; }
    
    public Guid? BranchId { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? LastModeifierId { get; set; }
   
    public string? EntityName { get; set; }

    public DateTime DateTime { get; set; }
} 