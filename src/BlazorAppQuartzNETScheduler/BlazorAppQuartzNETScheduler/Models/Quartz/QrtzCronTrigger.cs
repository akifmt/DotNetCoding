namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzCronTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public string CronExpression { get; set; } = null!;
    public string? TimeZoneId { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; } = null!;
}