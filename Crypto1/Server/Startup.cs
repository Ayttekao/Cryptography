using MessagePack;
using Microsoft.AspNetCore.Http.Connections;

namespace Server
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSignalR(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.MaximumReceiveMessageSize = null;
                    options.ClientTimeoutInterval = TimeSpan.FromSeconds(300);
                })
                .AddMessagePackProtocol(options =>
                {
                    options.SerializerOptions = 
                        MessagePackSerializerOptions.Standard
                        .WithSecurity(MessagePackSecurity.UntrustedData);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<FileTransferHub>("/chat", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
            });
            
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello world!");
            });
        }
    }
}