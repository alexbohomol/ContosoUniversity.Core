namespace ContosoUniversity.Mvc.Middleware;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;

internal class EnrichMetricsWithMvcLabels : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var tagsFeature = context.Features.Get<IHttpMetricsTagsFeature>();
        if (tagsFeature is not null)
        {
            tagsFeature.TryAddRouteValue(context, "controller");
            tagsFeature.TryAddRouteValue(context, "action");
        }

        await next(context);
    }
}

static file class Extensions
{
    public static void TryAddRouteValue(this IHttpMetricsTagsFeature feature, HttpContext context, string name)
    {
        var value = context.GetRouteValue(name);
        if (value is not null)
        {
            feature.Tags.Add(new KeyValuePair<string, object>($"mvc.{name}", value));
        }
    }
}
