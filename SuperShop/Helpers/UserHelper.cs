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

        //Construtor
        //Ctrl  + . em cima do userManager e clicar em "Create and assign field userManager"
        //Ctrl  + . em cima do signInManager e clicar em "Create and assign field signInManager"
        public UserHelper(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        //Método para criar utilizadores
        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(
            User user, 
            string oldPassword, 
            string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
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
    }
}
