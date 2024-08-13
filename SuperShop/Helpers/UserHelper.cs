using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public class UserHelper : IUserHelper
    {
        //Classe que faz a gestao dos utilizadores. Como é uma classe do .NetCore nao necessitamos de injetar no Startup.cs
        private readonly UserManager<User> _userManager;
        //Classe que faz a gestao dos SignIn's.Como é uma classe do .NetCore nao necessitamos de injetar no Startup.cs
        private readonly SignInManager<User> _signInManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        //Construtor
        //Ctrl  + . em cima do userManager e clicar em "Create and assign field userManager"
        //Ctrl  + . em cima do signInManager e clicar em "Create and assign field signInManager"
        public UserHelper(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            //Ctrl  + . em cima do roleManager e clicar em "Create and assign field roleManager"
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        //Método para criar utilizadores
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        //Roles - Estamos a adicionar o Role ao User
        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user, 
            string oldPassword, 
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        //Roles
        public async Task CheckRoleAsync(string roleName)
        {//Ver se o Role existe, caso exista vai busca-lo roleName
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            //Se ele nao existe
            if (!roleExists)
            {//Cria o Role
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        //Roles - Ve se o user tem determinado Role
        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName); 
        }

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                //Se metermos true nesta opçao ao enganarmo-nos na pass ja
                //nao nos deixa fazer uma segundo tentativa
                false);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        // Método que valida a password de um utilizador.
        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }
    }
}
