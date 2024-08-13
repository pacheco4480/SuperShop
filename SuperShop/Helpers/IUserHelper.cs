using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IUserHelper
    {
        // Método que recebe um email e devolve o utilizador correspondente a esse email, caso ele exista.
        Task<User> GetUserByEmailAsync(string email);

        // Método que adiciona um novo utilizador ao sistema, recebendo um objeto User e a senha para o registo.
        Task<IdentityResult> AddUserAsync(User user, string password);

        // Método que tenta fazer o login de um utilizador com base nas informações de LoginViewModel.
        // Devolve um resultado de SignIn que indica se o login foi bem-sucedido ou não.
        Task<SignInResult> LoginAsync(LoginViewModel model);

        // Método que realiza o logout do utilizador atual.
        Task LogoutAsync();

        // Método para atualizar as informações de um utilizador existente no sistema.
        Task<IdentityResult> UpdateUserAsync(User user);

        // Método para alterar a senha de um utilizador, recebendo a senha antiga e a nova senha.
        Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

        //Rolles - Método que verifica se um determinado Role (papel) existe no sistema e, se não existir, cria-o.
        Task CheckRoleAsync(string roleName);

        //Rolles - Método que adiciona um papel específico a um utilizador.
        Task AddUserToRoleAsync(User user, string roleName);

        //Rolles - Método que verifica se um utilizador já possui um papel específico atribuído.
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        // Método que valida a senha de um utilizador, devolvendo o resultado da validação.
        Task<SignInResult> ValidatePasswordAsync(User user, string password);
    }
}
