namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzSimpropTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string? StrProp1 { get; set; }
    public string? StrProp2 { get; set; }
    public string? StrProp3 { get; set; }
    public long? IntProp1 { get; set; }
    public long? IntProp2 { get; set; }
    public long? LongProp1 { get; set; }
    public long? LongProp2 { get; set; }
    public byte[]? DecProp1 { get; set; }
    public byte[]? DecProp2 { get; set; }
    public byte[]? BoolProp1 { get; set; }
    public byte[]? BoolProp2 { get; set; }
    public string? TimeZoneId { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; } = null!;
}