using GloboClima.Infra.Data.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GloboClima.Application
{
    public class Startup
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {

            // Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            //Cors
            services.AddCors(obj => obj.AddPolicy("PolicyDefault", UriBuilder =>
            {
                UriBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            // HTTPClient
            services.AddHttpClient<WeatherApiService>();
            services.AddHttpClient<CountryApiService>();

        }
    }
}
