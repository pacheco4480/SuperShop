using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SuperShop.Data.Entities
{
    //Esta classe IdentityUser ja tem propriedades predefinidas mas vamos acrescentar mais propriedades
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name ="Full Name")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
