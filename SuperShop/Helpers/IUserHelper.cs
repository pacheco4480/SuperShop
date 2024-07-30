using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {   //Recebe o email e dá o user correspondente ao email, caso ele exista
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync (User user, string password);

        //Criamos um metodo que devolve uma tarefa do tipo SignInResult (na pratica entra ou nao entra)
        Task<SignInResult> LoginAsync(LoginViewModel model);

        //O metodo LogoutAsync nao tem parametro nenhum (sai)
        Task LogoutAsync();
    }
}
