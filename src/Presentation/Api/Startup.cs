using ScopicAuctionSystem.Infrastructure;

namespace Api
{
    using Application;
    using Application.Users.Commands.CreateUser;
    using Extensions;
    using FluentValidation.AspNetCore;
    using Hubs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Middlewares;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using Persistence;
    using Services.Hosted;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services
                .AddPersistence(Configuration)
                .AddHostedService<MigrateDatabaseHostedService>()
                .AddInfrastructure(Configuration)
                .AddApplication()
                .AddCloudinarySettings(Configuration)
                .AddSendGridSettings(Configuration)
                .AddJwtAuthentication(services.AddJwtSecret(Configuration))
                .AddRequiredServices()
                .AddRedisCache(Configuration)
                .AddSwagger()
                .AddCors(options =>
                {
                    options.AddDefaultPolicy(
                        builder =>
                        {
                            builder.WithOrigins("http://localhost:4200",
                                "https://localhost:4200");
                            builder.AllowCredentials();
                            builder.AllowAnyMethod();
                            builder.AllowAnyHeader();
                        });
                })
                .AddControllers()
                .AddNewtonsoftJson(options => options.UseCamelCasing(true))
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserCommandValidator>());

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app
                .UseHttpsRedirection()
                .UseCustomExceptionHandler()
                .UseAuthorizationHeader()
                .UseRouting()
                .UseHsts()
                .UseCors()
                .UseAuthentication()
                .UseAuthorization()
                .UseSwaggerUi()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                    endpoints.MapHub<BidHub>("/bidHub");
                });
        }
    }
}