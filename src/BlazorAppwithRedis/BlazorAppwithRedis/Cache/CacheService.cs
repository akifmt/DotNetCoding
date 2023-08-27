using StackExchange.Redis;
using System.Text.Json;

namespace BlazorAppwithRedis.Cache;

public enum ConnectionStates
{
    Unknown = 0, Connected = 1, NoConnection = 2,
}

public class CacheService : ICacheService
{
    private IDatabase? _db;
    private ConnectionStates _connectionState = ConnectionStates.Unknown;
    private string _currentConnectionStatusMessage;

    public CacheService()
    {
        try
        {
            _db = ConnectionHelper.Connection.GetDatabase(0);
            _connectionState = ConnectionStates.Connected;
            _currentConnectionStatusMessage = "Connected.";
        }
        catch (Exception ex)
        {
            _connectionState = ConnectionStates.NoConnection;

            string errorMessage = ex.Message;

            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                errorMessage += Environment.NewLine + innerEx.Message;
                innerEx = innerEx.InnerException;
            }

            _currentConnectionStatusMessage = errorMessage;
        }
    }

    public (ConnectionStates ConnectionState, string Message) GetConnectionState() => (_connectionState, _currentConnectionStatusMessage);

    public TimeSpan? GetTTL(string key)
    {
        if (_connectionState != ConnectionStates.Connected) return null;
        var value = _db.KeyTimeToLive(key, CommandFlags.PreferMaster);
        if (value != null)
        {
            return value;
        }
        return default;
    }

    public T? GetDataMaster<T>(string key)
    {
        if (_connectionState != ConnectionStates.Connected) return default;
        var value = _db.StringGet(key, CommandFlags.PreferMaster);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value!);
        }
        return default;
    }

    public T? GetDataSlave<T>(string key)
    {
        if (_connectionState != ConnectionStates.Connected) return default;
        var value = _db.StringGet(key, CommandFlags.PreferReplica);
        if (!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value!);
        }
        return default;
    }

    public bool? SetDataMaster<T>(string key, T value, DateTimeOffset expirationTime)
    {
        if (_connectionState != ConnectionStates.Connected) return default;
        TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        var isSet = _db.StringSet(key, JsonSerializer.Serialize(value), expiryTime, flags: CommandFlags.PreferMaster);
        return isSet;
    }

    public object? RemoveDataMaster(string key)
    {
        if (_connectionState != ConnectionStates.Connected) return null;
        bool _isKeyExist = _db.KeyExists(key);
        if (_isKeyExist == true)
        {
            return _db.KeyDelete(key, CommandFlags.PreferMaster);
        }
        return false;
    }
}