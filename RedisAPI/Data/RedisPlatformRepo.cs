using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data;
public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    public void CreatePlatform(Platform platform)
    {
        if(platform is null)
            throw new ArgumentException(nameof(platform));

        var db = _redis.GetDatabase();

        var serialPlat = JsonSerializer.Serialize(platform);

        db.HashSet("hashPlatform", new HashEntry[] {new HashEntry(platform.Id, serialPlat)});
    }

    public IEnumerable<Platform>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase();

        var completeHash = db.HashGetAll("hashPlatform"); 

        if(completeHash.Length > 0)
        {
            var obj = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value)).AsEnumerable();
            return obj;
        }

        return null;
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();

        var platform = db.HashGet("hashPlatform", id);

        if(!string.IsNullOrEmpty(platform))
            return JsonSerializer.Deserialize<Platform>(platform);

        return null;
    }
}
