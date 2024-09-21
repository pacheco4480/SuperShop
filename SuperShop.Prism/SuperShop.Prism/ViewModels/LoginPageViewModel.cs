using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Prism.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {   
        //Atributos
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _loginCommand;


        //Construtor
        public LoginPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
        }

        //Comandos
        //Se este atributo _loginCommand ja tem um objeto instanciado nao faz nada apenas executa-o mas se este
        //atributo nao tiver um objeto instanciado instancia-o aqui "_loginCommand = new DelegateCommand()"
        //Este objeto _loginCommand quando clicado vai executar um método que será o Login
        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));



        //Propriedades
        //Quando quisermos só ler da view basta fazermos desta forma, quando quisermos
        //ler e escrever na view temos que fazer como está em baixo em public string Password 
        public string Email { get; set; }

        public string Password 
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        //Métodos
        private async void Login()
        {

            if (string.IsNullOrEmpty(Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an email.", "Accept");
                Password = string.Empty;
                return;
            }
            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "You must enter an password.", "Accept");
                Password = string.Empty;
                return;
            }

            await App.Current.MainPage.DisplayAlert("Ok", "Boa, entrámos!!", "Accept");
        }

    }
}
