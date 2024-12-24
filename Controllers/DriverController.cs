using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using my_redis.Data;
using my_redis.Models;
using my_redis.Services;

namespace my_redis.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    private ApplicationDbContext _context;

    private ICacheService _service;
    public DriverController(ICacheService service, ApplicationDbContext context)
    {
        _context = context;
        _service = service;
    }
    [HttpGet("drivers")]
    public async Task<IActionResult> GetDrivers() {

        var result = await _service.GetData<IEnumerable<Driver>>("drivers");
        if (result is not null && result.Count() > 0)
            return Ok(result);
        var drivers = await _context.Drivers.ToListAsync();
        var expirayTime = DateTimeOffset.Now.AddSeconds(30);
        await _service.SetData<IEnumerable<Driver>>("drivers", drivers, expirayTime);
        return Ok(drivers);
    }
    [HttpGet("driver/{id}")]
    public async Task<IActionResult> GetDriver(int id) {
        var result = await _service.GetData<Driver>($"driver-{id}");
        if (result is not null )
            return Ok(result);
        var driver = await _context.Drivers.SingleOrDefaultAsync(d=>d.Id == id);
        var expirayTime = DateTimeOffset.Now.AddSeconds(120);
        await _service.SetData<Driver>($"driver-{id}", driver, expirayTime);
        return Ok(driver);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddDriver(Driver driver) {


        var addedObj = await _context.Drivers.AddAsync(driver);
        var expirayTime = DateTimeOffset.Now.AddSeconds(30);
        await _service.SetData<Driver>($"driver-{driver.Id}", addedObj.Entity, expirayTime);
        await _context.SaveChangesAsync();
        return Ok(addedObj.Entity);
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteDriverFromCache(int id) {
        var result = await _service.GetData<Driver>($"driver-{id}");
        if (result is not null) {
            await _service.RemoveKey($"driver-{id}");
            
            return NoContent();
        }
        return NotFound();
    }
}
