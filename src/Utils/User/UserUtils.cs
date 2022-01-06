using Microsoft.Extensions.Options;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;

namespace BMS.Utils.User;

public class UserUtils : IUserUtils
{
    private readonly UserSecretOptions _secretOptions;

    public UserUtils(IOptions<UserSecretOptions> secretOptions)
    {
        _secretOptions = secretOptions.Value;
    }

    public string GenerateUserJwtToken(long userId)
    {
        var algorithm = new HMACSHA256Algorithm();

        var token = new JwtBuilder()
            .WithAlgorithm(algorithm)
            .AddClaim(Constants.UserIdKey, userId)
            .ExpirationTime(DateTime.UtcNow.AddDays(30))
            .WithSecret(_secretOptions.Secret)
            .Encode();

        return token;
    }

    public bool TryValidateAuthToken(string token, out UserAuthDto authDto)
    {
        authDto = null;
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            authDto = new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(_secretOptions.Secret)
                .MustVerifySignature()
                .Decode<UserAuthDto>(token);
        }
        catch (TokenExpiredException)
        {
            return false;
        }
        if (authDto is null || authDto.UserId < 1)
        {
            return false;
        }
        return true;
    }
}
