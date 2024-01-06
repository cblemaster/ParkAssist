using ParkAssist.API.Models.DTO;
using ParkAssist.Core;
using System.Text;

namespace ParkAssist.API.Models.Validation
{
    public static class DTOValidators
    {
        public static bool LogInUserDTOIsValid(LogInUserDTO logInUser)
        {
            bool usernameIsValid = GuardClauses.StringContainsChars(logInUser.Username) && GuardClauses.StringLengthInRangeInclusive(1, 50, logInUser.Username);
            bool passwordIsValid = GuardClauses.StringContainsChars(logInUser.Password) && GuardClauses.StringLengthInRangeInclusive(1, 200, logInUser.Password);
            return usernameIsValid && passwordIsValid;
        }

        public static (bool IsValid, string ErrorMessage) RegisterUserDTOIsValid(RegisterUserDTO registerUser)
        {
            StringBuilder errorMessage = new();
                        
            bool usernameIsValid = GuardClauses.StringContainsChars(registerUser.Username) && GuardClauses.StringLengthInRangeInclusive(1, 50, registerUser.Username);
            bool passwordIsValid = GuardClauses.StringContainsChars(registerUser.Password) && GuardClauses.StringLengthInRangeInclusive(1, 200, registerUser.Password);
            
            bool firstNameIsValid = GuardClauses.StringContainsChars(registerUser.FirstName) && GuardClauses.StringLengthInRangeInclusive(1, 255, registerUser.FirstName);
            bool lastNameIsValid = GuardClauses.StringContainsChars(registerUser.LastName) && GuardClauses.StringLengthInRangeInclusive(1, 255, registerUser.LastName);
            
            bool emailIsValid = GuardClauses.StringContainsChars(registerUser.Email) && GuardClauses.StringLengthInRangeInclusive(1, 255, registerUser.Email);
            bool phoneIsValid = GuardClauses.StringContainsChars(registerUser.Phone) && GuardClauses.StringLengthInRangeInclusive(1, 10, registerUser.Phone);

            bool roleIsValid = GuardClauses.StringContainsChars(registerUser.Role);

            if (!usernameIsValid || !passwordIsValid)
            {
                errorMessage.Append("invalid username or password input");
            }

            if (!firstNameIsValid)
            {
                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("; ");
                }
                errorMessage.Append("invalid first name input");
            }

            if (!lastNameIsValid)
            {
                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("; ");
                }
                errorMessage.Append("invalid last name input");
            }

            if (!emailIsValid)
            {
                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("; ");
                }
                errorMessage.Append("invalid email input");
            }

            if (!phoneIsValid)
            {
                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("; ");
                }
                errorMessage.Append("invalid phone input");
            }

            if (!roleIsValid)
            {
                if (errorMessage.Length > 0)
                {
                    errorMessage.Append("; ");
                }
                errorMessage.Append("invalid role input");
            }

            return (usernameIsValid && passwordIsValid && firstNameIsValid 
                && lastNameIsValid && emailIsValid && phoneIsValid, errorMessage.ToString());
        }
    }
}
