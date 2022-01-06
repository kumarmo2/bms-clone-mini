namespace BMS.Utils.User;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class UserServiceCollectionExtensions
{
    public static void AddUserSecrets(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<UserSecretOptions>(config.GetSection(UserSecretOptions.Key));
    }
}

