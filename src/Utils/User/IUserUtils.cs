namespace BMS.Utils.User;


public interface IUserUtils
{
    string GenerateUserJwtToken(long userId);
    bool TryValidateAuthToken(string token, out UserAuthDto authDto);
}

