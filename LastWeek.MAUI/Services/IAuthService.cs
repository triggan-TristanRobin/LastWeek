using System;
using System.Threading.Tasks;
using LastWeek.Model;
using LastWeek.MAUI.Model;

namespace LastWeek.MAUI.Services
{
    public interface IAuthService
    {
        Task<User> Register(User registerModel);
        Task<User> Signin(UserSigninInfos signinInfos);
        Task Signout();
    }
}