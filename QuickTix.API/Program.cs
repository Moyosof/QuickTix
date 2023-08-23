using Microsoft.EntityFrameworkCore;
using QuickTix.API;
using QuickTix.Repo.Data;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        await ApplyMigrations(host.Services);

        await host.RunAsync();
    }
    private static async Task ApplyMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        await using var dbcontext1 = scope.ServiceProvider.GetService<QuickTixDbContext>();
        await using var dbcontext2 = scope.ServiceProvider.GetService<ApplicationDbContext>();

        await dbcontext2.Database.MigrateAsync();
        await dbcontext1.Database.MigrateAsync();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}