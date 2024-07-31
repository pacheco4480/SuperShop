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

        //Metodo para fazer Update do utilizador
        Task<IdentityResult> UpdateUserAsync(User user);

        //Método para mudar a password
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //Roles - Verifica se tem determinado Role se nao tem cria
        Task CheckRoleAsync(string roleName);
        //Roles - Adiciona o Role a um determinado User
        Task AddUserToRoleAsync(User user, string roleName);
        //Roles - Vê se o User ja tem Role ou nao
        Task<bool> IsUserInRoleAsync(User user, string roleName);
    }
}
