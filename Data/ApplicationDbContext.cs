using Microsoft.EntityFrameworkCore;
using my_redis.Models;

namespace my_redis.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Driver> Drivers{get;set;}
    public ApplicationDbContext(DbContextOptions options) :base(options){

    }
}
