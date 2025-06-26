using CrmIntegrationApp.Configurations;
using CrmIntegrationApp.Infrastructure.ApiClients;
using CrmIntegrationApp.Infrastructure.ApiClients.Impl;
using CrmIntegrationApp.Models.Mapping;
using CrmIntegrationApp.Services;
using CrmIntegrationApp.Services.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace CrmIntegrationApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(mc => mc.AddProfile(new MappingProfile()));

            services.Configure<CrmApiSettings>(Configuration.GetSection("CrmApi"));
            services.Configure<CxoneApiSettings>(Configuration.GetSection("CxoneApi"));
            services.Configure<WebhookJwtSettings>(Configuration.GetSection("CrmApi:WebhookJwt"));


            services.AddHttpClient();

            services.AddSingleton<ICrmAuthService, CrmAuthService>();
            services.AddSingleton<ICxoneAuthService, CxoneAuthService>();
 
            services.AddSingleton<ICrmApiClient, CrmApiClient>();
            services.AddSingleton<ICxoneApiClient, CxoneApiClient>();

    
            services.AddSingleton<ITicketProcessorService, TicketProcessorService>();
            services.AddSingleton<ITicketService, TicketService>();
            services.AddSingleton<IWebhookProcessorService, WebhookProcessorService>();
            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var jwtSettings = Configuration.GetSection("CrmApi:WebhookJwt");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                    };
                });
            services.AddAuthorization();

            services.AddControllers();


            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { 
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            

            services.AddHostedService<TicketBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSerilogRequestLogging();

            //if (env.IsDevelopment())
            //{
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
            //}
            //else
            //{

            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}