namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzLock
{
    public string SchedName { get; set; } = null!;
    public string LockName { get; set; } = null!;
}