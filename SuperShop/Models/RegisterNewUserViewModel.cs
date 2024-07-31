using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class RegisterNewUserViewModel
    {   //Vai ser obrigatorio preencher o FirstName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        //Vai ser obrigatorio preencher o LastName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        //Vai ser obrigatorio preencher o Username
        [Required]
        //O Utilizador terá que usar o email para se logar
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        //Vai ser obrigatorio preencher a Password
        [Required]
        //A password tera que ter no minimo 6 caracteres este valor tem que
        //ser condizente com o tamanho da passowrd que definimos no ficheiro Startup.cs
        [MinLength(6)]
        public string Password { get; set; }

        //Confirmaçao, que serve para o utilizador por a password 2 vexes
        [Required]
        //Aqui vai confirmar que o valor que o utilizador poem no segundo campo é igual ao valor que defeniu para a Password
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
