using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;
using LazyCache;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using NodaTime;

namespace Turret.Api.Behaviours;

public interface ICacheableQuery<out TResponse> : IRequest<TResponse>
{
    public string CacheKey { get; }
    public Duration? CacheDuration { get; }
    
}

public class CachingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery<TResponse>
{
    private readonly IAppCache _appCache;

    public CachingBehaviour(IAppCache appCache)
    {
        _appCache = appCache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var cacheKey = $"{typeof(TRequest).Name}#{request.CacheKey}";

        var response = await _appCache.GetOrAddAsync(
            cacheKey,
            async entry =>
            {
                entry.SlidingExpiration = request.CacheDuration?.ToTimeSpan();
                entry.Priority = request.CacheDuration.HasValue
                    ? CacheItemPriority.Normal
                    : CacheItemPriority.NeverRemove;
                return await next.Invoke();
            });

        return response;
    }
}