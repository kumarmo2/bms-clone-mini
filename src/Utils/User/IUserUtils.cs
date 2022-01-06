namespace BMS.Utils.User;


public interface IUserUtils
{
    string GenerateUserJwtToken(long userId);
}

