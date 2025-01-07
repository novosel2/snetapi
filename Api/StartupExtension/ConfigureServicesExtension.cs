using Api.Filters.ExceptionFilters;
using Core.BackgroundServices;
using Core.Data.Entities.Identity;
using Core.IRepositories;
using Core.IServices;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Api.StartupExtension
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("newPolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HandleExceptionFilter));
            });

            services.AddHttpContextAccessor();
            services.AddLogging();

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

            var databaseServer = Environment.GetEnvironmentVariable("DATABASE_SERVER") ?? "localhost";
            var databaseUser = Environment.GetEnvironmentVariable("DATABASE_USER") ?? "defaultUser";
            var databasePassword = Environment.GetEnvironmentVariable("DATABASE_PASSWORD") ?? "defaultPassword";

            var authConnection = config.GetConnectionString("AuthConnection")!
                .Replace("${DATABASE_SERVER}", databaseServer)
                .Replace("${DATABASE_USER}", databaseUser)
                .Replace("${DATABASE_PASSWORD}", databasePassword);

            var appConnection = config.GetConnectionString("AppConnection")!
                .Replace("${DATABASE_SERVER}", databaseServer)
                .Replace("${DATABASE_USER}", databaseUser)
                .Replace("${DATABASE_PASSWORD}", databasePassword);

            services.AddDbContext<AuthDbContext>(options 
                => options.UseSqlServer(authConnection));
            services.AddDbContext<AppDbContext>(options
                => options.UseSqlServer(appConnection));

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AuthDbContext>();

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

            services.AddSingleton<IBlobStorageService, BlobStorageService>();
            services.AddHostedService<UpdatePreviousFollowersService>();
            services.AddHostedService<UpdatePopularityScoresService>();

            return services;
        }
    }
}