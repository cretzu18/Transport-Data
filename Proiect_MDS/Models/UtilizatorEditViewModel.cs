using System.ComponentModel.DataAnnotations;

namespace Proiect_MDS.Models
{
    public class UtilizatorEditViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nume { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public RolUtilizator Rol { get; set; }
    }

}
