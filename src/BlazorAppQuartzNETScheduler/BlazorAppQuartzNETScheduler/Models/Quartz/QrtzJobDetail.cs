namespace BlazorAppQuartzNETScheduler.Models.Quartz;

public partial class QrtzJobDetail
{
    public QrtzJobDetail()
    {
        QrtzTriggers = new HashSet<QrtzTrigger>();
    }

    public string SchedName { get; set; } = null!;
    public string JobName { get; set; } = null!;
    public string JobGroup { get; set; } = null!;
    public string? Description { get; set; }
    public string JobClassName { get; set; } = null!;
    public byte[] IsDurable { get; set; } = null!;
    public byte[] IsNonconcurrent { get; set; } = null!;
    public byte[] IsUpdateData { get; set; } = null!;
    public byte[] RequestsRecovery { get; set; } = null!;
    public byte[]? JobData { get; set; }

    public virtual ICollection<QrtzTrigger> QrtzTriggers { get; set; }
}