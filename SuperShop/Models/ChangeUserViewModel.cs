using System.ComponentModel.DataAnnotations;

namespace SuperShop.Models
{
    public class ChangeUserViewModel
    {
        //Vai ser obrigatorio preencher o FirstName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //Vai ser obrigatorio preencher o LastName
        [Required]
        //Este Display faz com que depois o nome apareça separado 
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
