namespace Backend.Application.Common.Interfaces;

public interface IAuthTokenService
{
    public string GenerateToken(Guid userId);
}
