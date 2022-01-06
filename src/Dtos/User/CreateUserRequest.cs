namespace BMS.Dtos.User;


public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public void Deconstruct(out string email, out string firstName, out string lastName, out string password)
    {
        email = Email;
        firstName = FirstName;
        lastName = LastName;
        password = Password;
    }
}
