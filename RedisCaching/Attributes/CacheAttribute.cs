using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RedisCaching.Configurations;

using Microsoft.AspNetCore.Mvc;
using RedisCaching.Services;
namespace RedisCaching.Attributes;

public class CacheAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeToLiveSeconds;

    public CacheAttribute(int timeToLiveSeconds = 1000)
    {
        _timeToLiveSeconds = timeToLiveSeconds;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

        if (!RedisConfiguration.Enabled)
        {
            await next();
            return;
        }
        Console.WriteLine("--> Generating key from request...");
        var cacheKey = RedisCaching.Helper.GenerateCacheKeyFromRequest(context.HttpContext.Request);
        Console.WriteLine($"--> Cache Key: {cacheKey}");
        var cacheResponse = await cacheService.GetCacheAsync(cacheKey);

        if (!string.IsNullOrEmpty(cacheResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cacheResponse,
                ContentType = "application/json",
                StatusCode = 200
            };
            context.Result = contentResult;
            return;
        }

        var excutedContext = await next();
        if (excutedContext.Result is OkObjectResult objectResult)
        {

            var expiryTime = DateTimeOffset.Now.AddSeconds(_timeToLiveSeconds);
            await cacheService.SetCacheAsync(cacheKey, objectResult.Value, expiryTime);
        }
    }
}
