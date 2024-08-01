using Microsoft.AspNetCore.Http;

public sealed class UserIdProviderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdProviderService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? GetUserId()
    {
        if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "userId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
        }

        return null;
    }
}