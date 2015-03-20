using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BargainHunter.Startup))]
namespace BargainHunter
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfire(config =>
            {
                // Basic setup required to process background jobs.
                config.UseSqlServerStorage("BargainHunter");
                config.UseServer();
            });
        }
    }
}