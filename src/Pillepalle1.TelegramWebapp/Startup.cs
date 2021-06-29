using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Pillepalle1.TelegramWebapp.Model.Database;
using Pillepalle1.TelegramWebapp.Model.Identity;
using Pillepalle1.TelegramWebapp.Model.Bot;
using Pillepalle1.TelegramWebapp.MvcExtensions.Services;
using Telegram.Bot;

namespace Pillepalle1.TelegramWebapp
{
    public class Startup
    {
        IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Configuring Telegram Bot
            services.AddHostedService<RegisterWebhook>();
            services.AddHttpClient("tgwebhook")
               .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(_config["BOT_TOKEN"], httpClient));
            services.AddSingleton<ITelegramBotUpdateHandler, TelegramBotUpdateHandlerImpl>();

            // Configuring database connection
            services.AddDbContextPool<ApplicationDbContext>(
                options => options.UseNpgsql(_config["DB_CONNECTION_STRING"]));

            // Configuring for account management
            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication();

            // Configuring MVC and NewtonsoftJson on which Telegram.Bot is highly dependent
            services.AddMvc()
                    .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ----------------------------------------------------------------
            // run ef core database migration every time the app starts
            // feels hacky and this is properly not the right place to call it
            // but according to 
            // https://entityframeworkcore.com/knowledge-base/48617880/ef-core-migrations-in-docker-container
            // this is the way to do it

            using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }

            // ----------------------------------------------------------------
            // from here onwards the request processing pipeline is created

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                // setting up the route for the bot
                var token = _config["BOT_TOKEN"];

                endpoints.MapControllerRoute(
                    "tgwebhook",
                    $"bot/{token}",
                    new { controller = "Webhook", Action = "Post" });

                // setting up routes for controllers
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
