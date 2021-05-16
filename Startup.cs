using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AuthGold.Database;
using Microsoft.EntityFrameworkCore;
using AuthGold.Contracts;
using AuthGold.Models;
using AuthGold.Providers;
using AuthGold.Filters;

namespace AuthGold
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration.GetConnectionString("DefaultConnectionMySQL");
            
            services.AddDbContext<Context>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));
            
            services.AddTransient<IRequestTrace, RequestTraceProvider>();
            services.AddTransient<IElapsedTime, ElapsedTimeProvider>();
            services.AddTransient<ICustomer, CustomerProvider>();
            services.AddTransient<IJsonManipulate, WriteJsonProvider>();
            services.AddTransient<IAESEncryptation, AESEncryptProvider>();
            services.AddTransient<AnythingFilter>();
            services.AddMvc(options =>
            {
                options.Filters.AddService<AnythingFilter>();
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=RequesTrace}/{action=Index}");
            });
        }
    }
}
