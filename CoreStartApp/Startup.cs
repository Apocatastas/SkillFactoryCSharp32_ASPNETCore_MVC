using CoreStartApp.Middlewares;
using Microsoft.AspNetCore.Builder;


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
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //Используем метод Use, чтобы запрос передавался дальше по конвейеру
            app.Use(async (context, next) =>
            {
                // Строка для публикации в лог
                string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

                // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
                string logFilePath = Path.Combine(env.ContentRootPath, "Logs", "RequestLog.txt");

                // Используем асинхронную запись в файл
                await File.AppendAllTextAsync(logFilePath, logMessage);

                await next.Invoke();
            });

            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(
                                endpoints => endpoints.MapGet("/", async context => {
                                                                                        await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
                                                                                    }
                                                             )

                            );
            app.Map("/about", About);
            app.Map("/config", Config);
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found");
            });
        }

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{_env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }

        /// <summary>
        ///  Обработчик для главной страницы
        /// </summary>
        private void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {_env.ApplicationName}. App running configuration: {_env.EnvironmentName}");
            });
        }

    }
}
