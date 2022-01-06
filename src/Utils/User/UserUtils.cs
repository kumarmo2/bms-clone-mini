using Microsoft.Extensions.Options;
using JWT.Algorithms;
using JWT.Builder;

namespace BMS.Utils.User;

public class UserUtils : IUserUtils
{
    private readonly UserSecretOptions _secretOptions;
    private const string _userIdKey = "userid";

    public UserUtils(IOptions<UserSecretOptions> secretOptions)
    {
        _secretOptions = secretOptions.Value;
    }

    public string GenerateUserJwtToken(long userId)
    {
        var algorithm = new HMACSHA256Algorithm();

        var token = new JwtBuilder()
            .WithAlgorithm(algorithm)
            .AddClaim(_userIdKey, userId)
            .ExpirationTime(DateTime.UtcNow.AddDays(30))
            .WithSecret(_secretOptions.Secret)
            .Encode();

        return token;
    }
}
