using System;

namespace LastWeek.Model
{
    public class User : Entity
    {
        public virtual string Email { get; set; }
        public virtual string Username { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public virtual string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }

        public User WithoutPassword()
        {
            var newUser = this.MemberwiseClone() as User;
            newUser.Password = null;
            return newUser;
        }

        public User WithHashedPassword(string hash)
        {
            var newUser = this.MemberwiseClone() as User;
            newUser.Password = hash;
            return newUser;
        }

        public void Update(User changedUserCreds)
        {
            Email = !string.IsNullOrEmpty(changedUserCreds.Email) ? changedUserCreds.Email : Email;
            Username = !string.IsNullOrEmpty(changedUserCreds.Username) ? changedUserCreds.Username : Username;
            FirstName = !string.IsNullOrEmpty(changedUserCreds.FirstName) ? changedUserCreds.FirstName : FirstName;
            LastName = !string.IsNullOrEmpty(changedUserCreds.LastName) ? changedUserCreds.LastName : LastName;
        }
    }
}