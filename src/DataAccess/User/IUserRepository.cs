namespace BMS.DataAccess.User;

using um = BMS.Models.User;

public interface IUserRepository
{
    Task Create(um.User user);
    Task<um.User> GetByEmail(string email);
}

