namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzBlobTrigger
{
    public string SchedName { get; set; } = null!;
    public string TriggerName { get; set; } = null!;
    public string TriggerGroup { get; set; } = null!;
    public byte[]? BlobData { get; set; }

    public virtual QrtzTrigger QrtzTrigger { get; set; } = null!;
}