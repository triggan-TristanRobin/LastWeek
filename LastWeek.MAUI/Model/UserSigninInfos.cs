using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace LastWeek.MAUI.Model
{
    public class UserSigninInfos
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }

        public bool IsUsernameEmail()
        {
            if (Username == null || !Username.Contains('@')) return false;
            try
            {
                MailAddress m = new(Username);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}