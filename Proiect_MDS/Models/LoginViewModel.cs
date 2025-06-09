using System.ComponentModel.DataAnnotations;

namespace Proiect_MDS.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Parola { get; set; }
    }
}
