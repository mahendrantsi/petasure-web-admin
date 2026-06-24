namespace Project.Middleware.Extensions
{
    using System;
    using Project.Models.APIModel;
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        // / <summary>
        // / Uses the common response middleware.
        // / You can exclude certain URL paths by adding (most of) the path to ResponseFormatExclude.
        // / </summary>
        // / <param name="builder">The application builder.</param>
        // / <param name="configureOptions">The configure options.</param>
        // / <returns>
        // /   <see cref="IApplicationBuilder" />.
        // / </returns>
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder, Action<ApiMiddlewareOptions> configureOptions)
        {
            var options = new ApiMiddlewareOptions();
            configureOptions(options);

            builder.UseMiddleware<RequestDurationMiddleware>();

            builder.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
            {
                appBuilder.UseMiddleware<CommonResponseMiddleware>(options);
            });

            return builder;
        }
    }
}
