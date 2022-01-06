using CommonLibs.Database;
using um = BMS.Models.User;
using Dapper;

namespace BMS.DataAccess.User;


public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task Create(um.User user)
    {
        var query = $@"insert into users.users(id, firstname, lastname, email, hashedpass)
                      values
                      (@id, @firstname, @lastname, @email, @hashedpass);";

        using (var conn = _connectionFactory.GetDbConnection())
        {
            await conn.ExecuteAsync(query, new
            {
                id = user.Id,
                firstname = user.FirstName,
                lastname = user.LastName,
                email = user.Email,
                hashedpass = user.HashedPass
            });
        }
    }

    public async Task<um.User> GetByEmail(string email)
    {
        var query = "select * from users.users where email = @email limit 1";

        using (var conn = _connectionFactory.GetDbConnection())
        {
            return await conn.QueryFirstOrDefaultAsync<um.User>(query, new { email = email });
        }
    }
}

