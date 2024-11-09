
using AsyncKeyedLock;
using Microsoft.EntityFrameworkCore;
using RaceConditionSample.Interface;
using RaceConditionSample.Persistence;
using RaceConditionSample.Services;

namespace RaceConditionSample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        // main application dbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                serverDbContextOptionsBuilder =>
                {
                    var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                    serverDbContextOptionsBuilder.CommandTimeout(minutes);
                    serverDbContextOptionsBuilder.EnableRetryOnFailure();
                });
        });

        builder.Services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddSingleton<AsyncKeyedLocker<string>>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}