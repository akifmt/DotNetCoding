using BlazorAppRadzenNet8SerilogLogging.Models;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;

namespace BlazorAppRadzenNet8SerilogLogging.Data;

public class LoggerService
{
    private readonly ILogger<LoggerService> _logger;
    private readonly ApplicationLoggerDbContext _loggerDbContext;

    public LoggerService(ILogger<LoggerService> logger, ApplicationLoggerDbContext loggerDbContext)
    {
        _logger = logger;
        _loggerDbContext = loggerDbContext;
    }

    public async Task<Log?> GetLogByIdAsync(int id)
    {
        _logger.LogInformation($"Called GetLogByIdAsync", id);
        return await _loggerDbContext.Logs.FirstOrDefaultAsync(x => x.id == id);
    }

    public async Task<(IEnumerable<Log> Result, int TotalCount)> GetLogsAsync(string? filter = default, int? top = default, int? skip = default, string? orderby = default, string? expand = default, string? select = default, bool? count = default)
    {
        _logger.LogInformation($"Called GetLogsAsync");

        var query = _loggerDbContext.Logs.AsQueryable();

        if (!string.IsNullOrEmpty(filter))
            query = query.Where(filter);

        if (!string.IsNullOrEmpty(orderby))
            query = query.OrderBy(orderby);

        int totalCount = 0;
        if (count == true)
            totalCount = query.Count();

        IEnumerable<Log>? result;
        if (skip == null || top == null)
            result = await query.ToListAsync();
        else
            result = await query.Skip(skip.Value).Take(top.Value).ToListAsync();

        return (result, totalCount);
    }

    public async Task<bool> DeleteLogByIdAsync(int id)
    {
        _logger.LogInformation($"Called DeleteLogByIdAsync", id);

        var log = await _loggerDbContext.Logs.FirstOrDefaultAsync(x => x.id == id);
        if (log == null)
            return false;

        _loggerDbContext.Logs.Remove(log);
        await _loggerDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool?> DeleteAllLogsAsync()
    {
        _logger.LogInformation($"Called DeleteAllLogsAsync");
        var all = await _loggerDbContext.Logs.ToListAsync();
        _loggerDbContext.Logs.RemoveRange(all); ;
        await _loggerDbContext.SaveChangesAsync();
        _logger.LogInformation($"Deleted All Logs.");
        return true;
    }
}