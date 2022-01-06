using BMS.DataAccess.User;
using BMS.Dtos.User;
using CommonLibs.Utils;
using CommonLibs.Utils.Id;
using um = BMS.Models.User;

namespace BMS.Business.User;

public class UserLogic : IUserLogic
{
    private readonly IUserRepository _userRepository;
    private readonly IIdFactory _idFactory;

    public UserLogic(IUserRepository userRepository, IIdFactory idFactory)
    {
        _userRepository = userRepository;
        _idFactory = idFactory;
    }

    public async Task<OneOf<Models.User.User, string>> CreateUser(CreateUserRequest request)
    {
        if (request is null)
        {
            throw new ArgumentNullException($"{nameof(request)}");
        }

        var existingUser = await _userRepository.GetByEmail(request.Email);

        if (existingUser is not null && existingUser.Id > 0)
        {
            return new OneOf<Models.User.User, string>("Email Address already exists");
        }

        var user = GetUser(request);
        await _userRepository.Create(user);
        return new OneOf<um.User, string>(user);
    }

    private um.User GetUser(CreateUserRequest request)
    {
        var hashedPass = BCrypt.Net.BCrypt.HashPassword(request.Password, 11);
        return new um.User
        {
            Id = _idFactory.Next(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashedPass = hashedPass,
            Email = request.Email,
        };
    }
}

