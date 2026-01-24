namespace ContosoUniversity.Mvc.Middleware;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

internal class EnrichMetricsWithMvcLabels : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var tagsFeature = context.Features.GetRequiredFeature<IHttpMetricsTagsFeature>();
        var routeValuesFeature = context.Features.GetRequiredFeature<IRouteValuesFeature>();

        if (routeValuesFeature.RouteValues.TryGetValue("controller", out object controller))
        {
            tagsFeature.Tags.Add(new KeyValuePair<string, object>("mvc.controller", controller));
        }

        if (routeValuesFeature.RouteValues.TryGetValue("action", out object action))
        {
            tagsFeature.Tags.Add(new KeyValuePair<string, object>("mvc.action", action));
        }

        await next(context);
    }
}
