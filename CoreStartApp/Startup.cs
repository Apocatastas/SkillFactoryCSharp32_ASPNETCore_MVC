using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;

namespace CoreStartApp
{
    public class Startup
    {
        IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(
                                endpoints => endpoints.MapGet("/", async context => {
                                                                                        await context.Response.WriteAsync("Hello, World!");
                                                                                    }
                                                             )

                            );
            app.UseEndpoints(
                    endpoints => endpoints.MapGet("/about", async context => {
                        await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                    }
                                                 )

                );
        }

    }
}
