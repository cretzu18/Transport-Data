using System.ComponentModel.DataAnnotations;

namespace Proiect_MDS.Models
{
    public enum RolUtilizator
    {
        Admin,
        Researcher,
        User
    }

    public class Utilizator
    {
        public int Id { get; set; }

        [Required]
        public string Nume { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string ParolaHashed { get; set; }

        [Required]
        public RolUtilizator Rol { get; set; }
    }
}
