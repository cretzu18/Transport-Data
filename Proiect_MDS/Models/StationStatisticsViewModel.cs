namespace Proiect_MDS.Models
{
    public class StationStatisticsViewModel
    {
        public int TotalStations { get; set; }
        public double AverageCongestion { get; set; }
        public Dictionary<float, int> Distribution { get; set; } = new();
    }
}
