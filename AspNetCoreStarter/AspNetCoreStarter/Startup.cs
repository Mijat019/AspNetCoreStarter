using AspNetCoreStarter.Context;
using AspNetCoreStarter.Repositories.Implementations;
using AspNetCoreStarter.Repositories.Interfaces;
using AspNetCoreStarter.Services;
using AspNetCoreStarter.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreStarter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AspNetCoreStarterContext>(
                options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

            services.AddSingleton(Configuration.GetSection("AppSettings").Get<AppSettings>());

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();

            services.AddScoped<UserService>();
            services.AddScoped<AuthService>();
            services.AddScoped<TodoService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
