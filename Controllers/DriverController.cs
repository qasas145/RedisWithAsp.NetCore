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
        await _service.SetData<IEnumerable<Driver>>("drivers", drivers);
        return Ok(drivers);
    }
}
