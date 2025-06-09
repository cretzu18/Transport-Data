using System.ComponentModel.DataAnnotations;

namespace Proiect_MDS.Models
{
    public class Station
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Range(1, 10, ErrorMessage = "Congestion level must be between 1 and 10.")]
        public float CongestionLevel { get; set; }
    }
}
