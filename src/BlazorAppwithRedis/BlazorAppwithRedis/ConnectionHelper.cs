using ServiceStack.Redis;
using StackExchange.Redis;

namespace BlazorAppwithRedis;

public class ConnectionHelper
{
    private static readonly IRedisClientsManager _clientsManager;
    private static readonly IRedisClient _redisClientMaster;
    private static readonly IRedisClient _redisClientSlave;

    static ConnectionHelper()
    {
        _clientsManager = new PooledRedisClientManager(ConfigurationManager.AppSetting["RedisURLMaster"], ConfigurationManager.AppSetting["RedisURLSlave"]);
        _redisClientMaster = _clientsManager.GetClient();
        _redisClientSlave = _clientsManager.GetReadOnlyClient();
        ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            string urls = $"{ConfigurationManager.AppSetting["RedisURLMaster"]},{ConfigurationManager.AppSetting["RedisURLSlave"]},allowAdmin=true";
            return ConnectionMultiplexer.Connect(urls);
        });
    }

    private static Lazy<ConnectionMultiplexer> lazyConnection;

    public static ConnectionMultiplexer Connection
    {
        get
        {
            return lazyConnection.Value;
        }
    }

    public static IEnumerable<IServer> GetServers()
    {
        return Connection.GetServers();
    }
}