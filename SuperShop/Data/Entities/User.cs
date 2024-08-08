using Microsoft.AspNetCore.Identity;

namespace SuperShop.Data.Entities
{
    //Esta classe IdentityUser ja tem propriedades predefinidas mas vamos acrescentar mais propriedades
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}
