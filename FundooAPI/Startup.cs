using BusinessLayer.Interface;
using BusinessLayer.Service;
using Confluent.Kafka;
using FundooAPI.RabitMQ.Interface;
using FundooAPI.RabitMQ.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Hashing;
using RepositoryLayer.Interface;
using RepositoryLayer.JwtToken;
using RepositoryLayer.Service;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace UserApi
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
            ConfigureDependencyInjection(services);

            ConfigureJwtAuthentication(services);

            services.AddControllers();

            AddSwaggerDocumentation(services);

            AddNlogConfiguration(services);

            AddRedisConfiguration(services);
            AddKafksConfiguration(services);
        }

        private void ConfigureDependencyInjection(IServiceCollection services)
        {
            services.AddScoped<IUserBL, UserBL>();
            services.AddScoped<IUserRL, UserRL>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<INotesBL, NotesBL>();
            services.AddScoped<INotesRL, NotesRL>();
            services.AddScoped<Password_Hash>();
            services.AddScoped<JwtToken>();
            services.AddScoped<IPublishSubscribeMQProducer, PublishSubscribeMQProducer>();

            services.AddDbContext<FundooApiContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.SaveToken = true;
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    });
        }

        private void AddSwaggerDocumentation(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Fundoo API",
                    Description = "Fundoo Api Management",
                    TermsOfService = new Uri("https://bridgelabz.com"),
                    Contact = new OpenApiContact
                    {
                        Name = "Prabhat",
                        Email = "Prabhat@bridgelabz.com",
                        Url = new Uri("https://bridgelabz.com"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://bridgelabz.com"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

                var filename = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var filepath = Path.Combine(AppContext.BaseDirectory, filename);
                c.IncludeXmlComments(filepath);
            });
        }

        private void AddNlogConfiguration(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Information);
                loggingBuilder.AddNLog("NLog.config");
            });
        }

        private void AddRedisConfiguration(IServiceCollection services)
        {
            string redisConnectionString = Configuration.GetConnectionString("Redis");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
            });
        }

        private void AddKafksConfiguration(IServiceCollection services)
        {
            var producerConfig = new ProducerConfig();
            Configuration.Bind("producer",producerConfig);
            services.AddSingleton<ProducerConfig>(producerConfig);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fundoo API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
