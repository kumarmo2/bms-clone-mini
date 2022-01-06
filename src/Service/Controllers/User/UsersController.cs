using BMS.Business.User;
using BMS.Dtos.User;
using Microsoft.AspNetCore.Mvc;
using BMS.Utils.User;
using cu = CommonLibs.Utils;
using BMS.Utils;
using um = BMS.Models.User;

// using BMS.Utils.User;

namespace BMS.Services.Controllers.User;


public class UsersController : CommonController
{
    private readonly IUserLogic _userLogic;
    private readonly IUserUtils _userUtils;

    public UsersController(IUserLogic userLogic, IUserUtils userUtils)
    {
        _userLogic = userLogic;
        _userUtils = userUtils;
    }


    [HttpPost("signup")]
    public async Task<ActionResult<cu.OneOf<UserDto, string>>> CreateUser(CreateUserRequest request)
    {
        if (request is null)
        {
            return BadRequest(new cu.OneOf<UserDto, string>("Must pass Body"));
        }

        var validationMessage = ValidateCreateUserRequest(request);
        if (validationMessage is not null)
        {
            return BadRequest(new cu.OneOf<UserDto, string>(validationMessage));
        }
        var result = await _userLogic.CreateUser(request);
        Console.WriteLine($"result is null: {result is null}");
        Console.WriteLine($"result.Ok is null: {result.Ok is null}");
        if (result is null)
        {
            return StatusCode(500);
        }

        if (!string.IsNullOrWhiteSpace(result.Err))
        {
            return new cu.OneOf<UserDto, string>(result.Err);
        }


        if (!string.IsNullOrWhiteSpace(result.Err))
        {
            return new cu.OneOf<UserDto, string>(result.Err);
        }

        var user = result.Ok;
        AddAuthCookie(user.Id);
        return new cu.OneOf<UserDto, string>(GetUserDto(user));
    }

    private static UserDto GetUserDto(um.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.Email,
            LastName = user.Email,
        };
    }

    private void AddAuthCookie(long userId)
    {
        var token = _userUtils.GenerateUserJwtToken(userId);
        Console.WriteLine($"token: {token}");
        var cookies = Response.Cookies;
        var cookieBuilder = new CookieBuilder();
        // cookieBuilder.Name = Constants.UserAuthCookieKey;
        cookieBuilder.Path = "/";
        cookieBuilder.Expiration = TimeSpan.FromDays(30);
        cookieBuilder.SameSite = SameSiteMode.Lax;

        var cookieOptions = cookieBuilder.Build(this.HttpContext);
        this.Response.Cookies.Append(Constants.UserAuthCookieKey, token, cookieOptions);
    }

    private static string ValidateCreateUserRequest(CreateUserRequest request)
    {
        var (email, firstName, lastName, password) = request;

        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email cannot be empty";
        }

        if (string.IsNullOrWhiteSpace(firstName))
        {
            return "FirstName cannot be empty";
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return "LastName cannot be empty";
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return "Password cannot be empty";
        }

        var trimmed = email.Trim();
        try
        {
            var addr = new System.Net.Mail.MailAddress(request.Email);
            var isValidEmail = addr.Address == trimmed;
            if (!isValidEmail)
            {
                return "Invalid Email Address";
            }
        }
        catch
        {
            return "Invalid Email Address";
        }

        return null;
    }
}
