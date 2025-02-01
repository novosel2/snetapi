
namespace Api.Middleware
{
    public class ProfileCacheControlMiddleware
    {
        private readonly RequestDelegate _next;

        public ProfileCacheControlMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/profiles"))
            {
                context.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
                context.Response.Headers.Pragma = "no-cache";
                context.Response.Headers.Expires = "-1";
            }

            await _next(context);
        }
    }

    public static class ProfileCacheControlMiddlewareExtensions
    {
        public static IApplicationBuilder UseProfileCacheControl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ProfileCacheControlMiddleware>();
        }
    }
}
