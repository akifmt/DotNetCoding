namespace BlazorAppSendEmailwithMailKit.Data;

public class EmailConfiguration
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string From { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool EnableSSL { get; set; }
}