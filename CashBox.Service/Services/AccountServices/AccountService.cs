using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CashBox.Service.Services;
public class IAccountService
{
    int UserId { get; }
    int OrganizationId { get; }
}
public class AccountService : IAccountService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?
                .Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return int.TryParse(claim?.Value, out var id) ? id : 0;
        }
    }

    public int OrganizationId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext?.User?
                .Claims
                .FirstOrDefault(c => c.Type == "OrganizationId");

            return int.TryParse(claim?.Value, out var id) ? id : 0;
        }
    }
}
