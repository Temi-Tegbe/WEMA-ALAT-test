using CustomerOnboarding.Domain.Helpers;
using CustomerOnboarding.Domain.Model;
using CustomerOnboarding.Services.Options;
using CustomerOnboarding.Services.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Microsoft.AspNetCore.Identity;
using CustomerOnboarding.Helpers;

namespace NEWCustomerOnboarding
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private AppSettings appSettings;
        private IConfigurationSection appSettingsSection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            appSettingsSection = Configuration.GetSection("AppSettings");
            appSettings = appSettingsSection.Get<AppSettings>();
        }

        //public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(appSettingsSection);
            services.AddControllers(
                options => options.Filters.Add(typeof(ExceptionFilters))
                ).AddNewtonsoftJson();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CustomerOnboarding", Version = "v1" });
                c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Token for Authorization"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
               {
                {
                        new OpenApiSecurityScheme
                {
                        Reference = new OpenApiReference
                {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Authorization"
                }

                        },
                new string[] {}
                }
               });
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = appSettings.JWT.ValidIssuer,
                            ValidAudience = appSettings.JWT.ValidAudience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JWT.Secret))
                        };
                    });
            CustomerOnboardingStartupMock.ConfigureServices(services);

            services.AddDbContext<AppDbContext>(options
                 => options.UseSqlServer(Configuration.GetConnectionString("ConnStr")));
            services.AddIdentity<ApplicationUser, ApplicationRole>()
              .AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();
            //services.AddScoped<CustomerService>();
            
            services.AddSingleton<Audit>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NEWAdminDashboard v1"));
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            loggerFactory.AddFile("Logs/mylog-{Date}.txt");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
