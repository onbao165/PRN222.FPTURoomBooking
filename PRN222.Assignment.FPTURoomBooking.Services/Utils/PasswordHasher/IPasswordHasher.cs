namespace PRN222.Assignment.FPTURoomBooking.Services.Utils.PasswordHasher;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}