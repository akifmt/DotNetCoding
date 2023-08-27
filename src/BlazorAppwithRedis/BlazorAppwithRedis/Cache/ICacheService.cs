namespace BlazorAppwithRedis.Cache;

public interface ICacheService
{
    (ConnectionStates ConnectionState, string Message) GetConnectionState();

    TimeSpan? GetTTL(string key);

    T? GetDataMaster<T>(string key);

    T? GetDataSlave<T>(string key);

    bool? SetDataMaster<T>(string key, T value, DateTimeOffset expirationTime);

    object? RemoveDataMaster(string key);
}