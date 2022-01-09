using BMS.Utils;
using BMS.Utils.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BMS.Services.Filters;


public class AuthFilter : IAuthorizationFilter
{
    private readonly IUserUtils _userUtils;
    public AuthFilter(IUserUtils userUtils)
    {
        _userUtils = userUtils;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var cookies = context.HttpContext.Request.Cookies;

        cookies.TryGetValue(Constants.UserAuthCookieKey, out var value);

        if (string.IsNullOrWhiteSpace(value))
        {
            context.Result = new StatusCodeResult(403);
            return;
        }
        if (!_userUtils.TryValidateAuthToken(value, out var authDto))
        {
            context.Result = new StatusCodeResult(403);
            return;
        }
        context.HttpContext.Items[Constants.AuthDtoKey] = authDto;
        return;

    }
}

