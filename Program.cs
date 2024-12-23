using AGSR.TestTask.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AGSR.TestTask;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var conectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AgsrContext>(o => o.UseNpgsql(conectionString));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
            app.UseSwagger();
            app.UseSwaggerUI();
        //}

        using (var serviceProvider = builder.Services.BuildServiceProvider())
        using (var context = serviceProvider.GetRequiredService<AgsrContext>())
        {
            context.Database.Migrate();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}