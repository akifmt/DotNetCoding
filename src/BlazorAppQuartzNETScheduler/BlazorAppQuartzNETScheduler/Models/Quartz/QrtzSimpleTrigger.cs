namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzSimpleTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public long RepeatCount { get; set; }
    public long RepeatInterval { get; set; }
    public long TimesTriggered { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; } = null!;
}