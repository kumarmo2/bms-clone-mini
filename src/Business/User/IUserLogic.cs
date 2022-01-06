using BMS.Dtos.User;
using um = BMS.Models.User;
using CommonLibs.Utils;

namespace BMS.Business.User;


public interface IUserLogic
{
    Task<OneOf<um.User, string>> CreateUser(CreateUserRequest request);
}
