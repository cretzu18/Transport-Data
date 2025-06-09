using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;


namespace Proiect_MDS.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Utilizator> Utilizatori { get; set; }
        public DbSet<Station> Stations { get; set; }
    }
}
