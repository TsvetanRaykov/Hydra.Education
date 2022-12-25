namespace Hydra.Server.Auth
{
    using Module.Video.Backend;
    using Blazorise;
    using Blazorise.Bootstrap;
    using Blazorise.Icons.FontAwesome;
    using Contracts;
    using Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Models;
    using Services;

    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;

                })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddHttpClient();
            services.AddRazorPages();
            
            services.AddServerSideBlazor()
                .AddCircuitOptions(options =>
                {
                    options.DetailedErrors = true;
                });

            services.AddBlazorise(options =>
                {
                    options.ChangeTextOnKeyPress = true; // optional
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();

            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IEmailSender, SendInBlueEmailSender>();

            // *** ADD MODULES
            services.AddHydraModuleVideo(options =>
            {
                Configuration.GetSection("Modules:Video").Bind(options);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            // *** USE MODULES
            app.UseHydraModuleVideo(opt =>
            {
                opt.StaticFilesLocation = Configuration["Modules:Video:StaticFilesLocation"];
            });

            app.MapWhen(c => c.Request.Path.StartsWithSegments("/Video"), app1 =>
            {
                app1.UseBlazorFrameworkFiles("/Video");
                app1.UseRouting();
                app1.UseAuthorization();
                app1.UseEndpoints(ep => ep.MapFallbackToController("Video", "Modules"));
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Server Side Blazor endpoints
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub("Auth/_blazor");
                endpoints.MapFallbackToPage("~/Auth/{*clientroutes:nonfile}", "/Auth/_Host");
            });
        }
    }
}
