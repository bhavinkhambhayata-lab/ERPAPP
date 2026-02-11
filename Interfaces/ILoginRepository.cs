using ERPAPP.Models;

namespace ERPAPP.Interfaces
{
    public interface ILoginRepository
    {
        public LoginResult Login(LoginViewModel model);
    }
}
