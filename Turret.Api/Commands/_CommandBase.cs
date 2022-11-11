using MediatR;

namespace Turret.Api.Commands;

public abstract record CommandBase<TResponse> : IRequest<TResponse>;
public abstract record CommandBase : CommandBase<Unit>;

public abstract class CommandHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> 
    where TRequest : CommandBase<TResponse>
{
    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public abstract class CommandHandlerBase<TRequest> : CommandHandlerBase<TRequest, Unit> 
    where TRequest : CommandBase
{
    public sealed override async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken = default)
    {
        await HandleUnit(request, cancellationToken);
        return Unit.Value;
    }

    public abstract Task HandleUnit(TRequest request, CancellationToken cancellationToken = default);
}