using BCrypt.Net;

namespace CinemaTicketBooking.Cryptography
{
    public class PasswordHasher
    {

        public string GenerateSalt()
        {   //generate a unique salt for each user
            return BCrypt.Net.BCrypt.GenerateSalt();
        }


        public string HashPassword(string password, string salt)
        {
            //hash the plain password using the given salt
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public bool IsPasswordValid(string enteredPassword, string storedHashedPassword, string storedSalt)
        {
            //verify if the entered password matches the stored hashed password
            string hashedPasswordToCheck = HashPassword(enteredPassword, storedSalt);
            return hashedPasswordToCheck == storedHashedPassword;
        }
    }
}
