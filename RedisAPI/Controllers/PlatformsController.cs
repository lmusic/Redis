using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repo;

    public PlatformsController(IPlatformRepo repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public ActionResult<Platform> GetPlatformById()
    {
        var platforms = _repo.GetAllPlatforms();

        return Ok(platforms);
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _repo.GetPlatformById(id);

        if(platform is not null)
            return Ok(platform);
        
        return NotFound();
    }

    [HttpPost]
    public ActionResult<Platform> CreatePlatform(Platform platform)
    {
        _repo.CreatePlatform(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new {Id = platform.Id}, platform);
    }
}