using Backend.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Backend.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger, ICurrentUser user, IIdentityService identityService)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        string? userName = null;
        if (user.Id is not null)
        {
            userName = await identityService.GetUserEmailAsync(user.Id.Value);
        }

        _logger.LogInformation("Backend Request: {@Name} {@UserId} {@UserName} {@Request}", requestName, user.Id, userName, request);
    }
}
