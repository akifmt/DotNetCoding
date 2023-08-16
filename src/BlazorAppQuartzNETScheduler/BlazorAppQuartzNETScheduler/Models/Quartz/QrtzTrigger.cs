namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string JobName { get; set; } = null!;
    public string JobGroup { get; set; } = null!;
    public string? Description { get; set; }
    public long? NextFireTime { get; set; }
    public long? PrevFireTime { get; set; }
    public long? Priority { get; set; }
    public string TriggerState { get; set; } = null!;
    public string TriggerType { get; set; } = null!;
    public long StartTime { get; set; }
    public long? EndTime { get; set; }
    public string? CalendarName { get; set; }
    public long? MisfireInstr { get; set; }
    public byte[]? JobData { get; set; }

    public virtual QrtzJobDetail QrtzJobDetail { get; set; } = null!;
    public virtual QrtzBlobTrigger? QrtzBlobTrigger { get; set; }
    public virtual QrtzCronTrigger? QrtzCronTrigger { get; set; }
    public virtual QrtzSimpleTrigger? QrtzSimpleTrigger { get; set; }
    public virtual QrtzSimpropTrigger? QrtzSimpropTrigger { get; set; }
}