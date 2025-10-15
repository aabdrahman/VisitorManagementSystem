using Contracts;
using Entities.Model;
using LoggerManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service.Contracts;
using Services;
using VisitorManagementSystem.Presentation.ActionFilters;
using System.Text;
using System.Threading.RateLimiting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace VisitorManagementSystem.Extensions;

public static class ServiceExtension
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("defaultPolicy", builder =>
            {
                builder.AllowAnyOrigin();
                builder.WithMethods("GET", "POST", "PUT", "OPTIONS");
                builder.WithHeaders("accept", "content-type")
                .WithHeaders("X-Pagination");
            });

            options.AddPolicy("FrontEndPolicy", builder =>
            {
                builder.WithMethods("GET", "POST", "PUT", "OPTIONS")
                        .WithOrigins("https://localhost:7034", "http://localhost:5285", "https://localhost:7126", "http://localhost:5235")
				        .AllowAnyHeader()
                        .WithExposedHeaders("X-Pagination");
            });
        });
    }

    public static void ConfigureIIS(this IServiceCollection services)
    {
        services.Configure<IISOptions>(options =>
        {

        });
    }

    public static void ConfigureLogggerService(this IServiceCollection services)
    {
        services.AddSingleton<ILoggerManager, AppLoggerManager>();
    }

    public static void ConfigureRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
    }

    public static void ConfigureDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>((provider, services) =>
        {
            var logger = provider.GetRequiredService<ILoggerFactory>();
            services.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
                                .UseLoggerFactory(logger)
                                .EnableSensitiveDataLogging();
        });
    }

    public static void ConfigureSqlConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(c => c.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
                                                            .EnableSensitiveDataLogging());
    }

    public static void ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }

    public static void ConfigureExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandller>();
    }

    public static void ConfigureIdentityContext(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;

        }).AddEntityFrameworkStores<RepositoryContext>()
          .AddDefaultTokenProviders();
    }

    public static void ConfigureActionFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsConfiguration = configuration.GetSection("JwtSettings");
        var secretKey = configuration["JwtSecretKey"];

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                ValidAudience = jwtSettingsConfiguration["ValidAudience"],
                ValidIssuer = jwtSettingsConfiguration["ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };

        });
    }

    public static void ConfigureJwtAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ReceptionistPolicy", p =>
            {
                p.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Receptionist", "Admin");
            });

            options.AddPolicy("AdminPolicy", p =>
            {
                p.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin");
            });

        });
    }

    public static void ConfigureOutputCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("300SecondsPolicy", p => p.Expire(TimeSpan.FromSeconds(300)));
        });
    }

    public static void ConfigureResponseCaching(this IServiceCollection services)
    {
        services.AddResponseCaching();
    }

    public static void ConfigureRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>
            (
                context => RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", partition => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 5,
                    AutoReplenishment = true,
                    QueueLimit = 2,
                    Window = TimeSpan.FromMinutes(1)
                }
            ));

            options.AddPolicy("SpecialPolicy", policyOptions =>

                RateLimitPartition.GetFixedWindowLimiter("SpecialLimiter", partition => new FixedWindowRateLimiterOptions
                {
                     PermitLimit = 10,
                     AutoReplenishment = true,
                     QueueLimit = 2,
                     Window = TimeSpan.FromSeconds(5)

                })
            );

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    await context.HttpContext.Response.WriteAsync($"Too many requests. Retry after: {retryAfter.TotalSeconds} seconds", token);
                else
                    await context.HttpContext.Response.WriteAsync($"Too many requests. Retry again later.", token);
            };
        });
    }

    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opts =>
        {
            opts.SwaggerDoc("v1", new OpenApiInfo { Title = "VisitorManagementSystemAPI", Description = "Visitor Management System API" });
            //opts.SchemaFilter<EnumSchemaFilter>();

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter your Token here.",
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                Type = SecuritySchemeType.ApiKey
            };

            opts.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        },
                        Name = "Bearer",
                    },
                    new List<string>()
                }
            };

            opts.AddSecurityRequirement(securityRequirement);
        });
    }

    public static void ConfigureEnumSerializer(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(o =>
        {
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
    }

    //public static  void ConfigureControllers(this IServiceCollection services)
    //{
    //    services.AddControllers(options =>
    //    {
    //        options.
    //    }).AddApplicationPart(typeof(VisitorManagementSystem.Presentation.AssemblyReference).Assembly);
    //}
}
