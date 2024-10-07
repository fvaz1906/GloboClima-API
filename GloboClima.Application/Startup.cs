using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using GloboClima.Domain.Entities;
using GloboClima.Domain.Interfaces;
using GloboClima.Infra.Data.Api;
using GloboClima.Infra.Data.Context;
using GloboClima.Infra.Data.Repository;
using GloboClima.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GloboClima.Application
{
    public class Startup
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {

            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                        ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")),
                        b => b.MigrationsAssembly("GloboClima.Infra.Data"))
                .UseLazyLoadingProxies().EnableSensitiveDataLogging());

            var credentials = new BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"]);
            var dynamoDbClient = new AmazonDynamoDBClient(credentials, RegionEndpoint.SAEast1);

            // Registra o DynamoDBContext para ser injetado nos serviços e controladores
            services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            //Cors
            services.AddCors(obj => obj.AddPolicy("PolicyDefault", UriBuilder =>
            {
                UriBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // HTTPClient
            services.AddHttpClient<IWeatherApiService, WeatherApiService>();
            services.AddHttpClient<ICountryApiService, CountryApiService>();

            // Repositories
            services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();

            // Services
            services.AddScoped<IBaseService<User>, BaseService<User>>();

        }
    }
}
