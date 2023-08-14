using BlazorAppSerilogLogging.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorAppSerilogLogging.Data;

public class LoggerService
{
    private readonly ILogger<LoggerService> _logger;
    private readonly ApplicationLoggerDbContext _loggerDbContext;

    public LoggerService(ILogger<LoggerService> logger, ApplicationLoggerDbContext loggerDbContext)
    {
        _logger = logger;
        _loggerDbContext = loggerDbContext;
    }

    public async Task<IEnumerable<Log>> GetLogsAsync()
    {
        _logger.LogInformation($"Called GetLogsAsync");
        return await _loggerDbContext.Logs.OrderByDescending(x => x.Timestamp).ToListAsync();
    }

    public async Task<Log?> GetLogAsync(int id)
    {
        _logger.LogInformation($"Called GetLogAsync", id);
        return await _loggerDbContext.Logs.FirstOrDefaultAsync(x => x.id == id);
    }

    public async Task<bool?> DeleteLogsAsync()
    {
        _logger.LogInformation($"Called DeleteLogsAsync");
        var all = await _loggerDbContext.Logs.ToListAsync();
        _loggerDbContext.Logs.RemoveRange(all); ;
        await _loggerDbContext.SaveChangesAsync();
        _logger.LogInformation($"Deleted All Logs.");
        return true;
    }
}