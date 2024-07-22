using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {   //Recebe o email e dá o user correspondente ao email, caso ele exista
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync (User user, string password);
    }
}
