using Microsoft.EntityFrameworkCore;
using RaceConditionSample.Entities;
using RaceConditionSample.Interface;
using System.Reflection;

namespace RaceConditionSample.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // it should be placed here, otherwise it will rewrite the following settings!
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}