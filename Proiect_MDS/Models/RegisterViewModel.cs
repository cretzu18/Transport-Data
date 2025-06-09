using System.ComponentModel.DataAnnotations;

namespace Proiect_MDS.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Nume { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Parola { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Parola", ErrorMessage = "The passwords are not matching.")]
        public string ConfirmParola { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public string Rol { get; set; }
    }
}
