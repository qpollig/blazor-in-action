using BlazingTrails.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using System.Reflection;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Add services to the container.
        services.AddDbContext<BlazingTrailsContext>(options =>
            options.UseSqlite(context.Configuration.GetConnectionString("BlazingTrailsContext")));
        
        services.AddControllers().AddFluentValidation(fv => 
            fv.RegisterValidatorsFromAssembly(Assembly.Load("BlazingTrails.Shared")));
    })
    .Build();

var app = host.Services.GetRequiredService<IHostApplicationLifetime>().Application;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();