using Api.Filters.ExceptionFilters;
using Core.BackgroundServices;
using Core.Data.Entities.Identity;
using Core.IRepositories;
using Core.IServices;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Cached;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Text;

namespace Api.StartupExtension
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            // Register Application Insights for telemetry and performance monitoring
            // This will enable automatic tracking of requests, dependencies, and other telemetry data for application performance insights.
            services.AddApplicationInsightsTelemetry();

            // Configure additional Application Insights options
            // Enabling Dependency and Request Tracking modules to capture telemetry for external service calls and HTTP requests.
            services.Configure<ApplicationInsightsServiceOptions>(options =>
            {
                options.EnableDependencyTrackingTelemetryModule = true;
                options.EnableRequestTrackingTelemetryModule = true;
            });

            // Add CORS policies, allow any origin so that it can be accessed from the frontend
            services.AddCors(options =>
            {
                options.AddPolicy("newPolicy", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            services.AddControllers(options =>
            {
                // Add a global ExceptionFilter, handles every exception thrown during the request
                options.Filters.Add(typeof(HandleExceptionFilter));
            });

            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddLogging();

            // Add Swagger, configure it for JWT authentication
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "To-Do API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            // Register a Distributed Redis Cache
            // This enables caching for the application
            services.AddStackExchangeRedisCache(setup =>
            {
                setup.Configuration = config.GetConnectionString("RedisConnection");
            });

            // Get database information from Environment Variables for dynamic configuration
            var databaseServer = Environment.GetEnvironmentVariable("DATABASE_SERVER") ?? "localhost";
            var databaseUser = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "defaultUser";
            var databasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "defaultPassword";

            // Create connection strings dynamically
            var authConnection = config.GetConnectionString("AuthConnection")!
                .Replace("${DATABASE_SERVER}", databaseServer)
                .Replace("${DATABASE_USER}", databaseUser)
                .Replace("${DATABASE_PASSWORD}", databasePassword);

            var appConnection = config.GetConnectionString("AppConnection")!
                .Replace("${DATABASE_SERVER}", databaseServer)
                .Replace("${DATABASE_USER}", databaseUser)
                .Replace("${DATABASE_PASSWORD}", databasePassword);

            // Add DbContexts for PostgreSQL database connections
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(authConnection));
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(appConnection));

            // Add Identity credentials requirements, also specify the DbContext used
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>();

            // Add authentication with JwtBearer
            // This ensures that only authenticated users can access protected resources
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme =
                options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = config["JWT:Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = config["JWT:Issuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SIGNING_KEY")!)),
                    ValidateLifetime = true
                };
            });

            // Register services and repositories with Dependency Injection (DI) container
            // This makes sure that the required dependencies for each service are automatically injected when needed.
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IPostReactionsService, PostReactionsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<ICommentReactionsService, CommentReactionsService>();
            services.AddScoped<IFriendRequestsService, FriendRequestsService>();
            services.AddScoped<IFriendshipsService, FriendshipsService>();
            services.AddScoped<IFollowsService, FollowsService>();

            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IPostsRepository, PostsRepository>();
            services.AddScoped<IPostReactionsRepository, PostReactionsRepository>();
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<ICommentReactionsRepository, CommentReactionsRepository>();
            services.AddScoped<IFriendRequestsRepository, FriendRequestsRepository>();
            services.AddScoped<IFriendshipsRepository, FriendshipsRepository>();
            services.AddScoped<IFollowsRepository, FollowsRepository>();

            //services.Decorate<IPostsRepository, CachedPostsRepository>();

            services.AddSingleton<IBlobStorageService, BlobStorageService>();

            // Register hosted services for background tasks
            // These services will run in the background of the application.
            services.AddHostedService<UpdatePreviousFollowersService>();
            services.AddHostedService<UpdatePopularityScoresService>();
            services.AddHostedService<RefreshApiActivityService>();

            return services;
        }
    }
}