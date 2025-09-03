using Microsoft.EntityFrameworkCore;

namespace challengeABD 
{
    public class MotoDB : DbContext
    {
        public DbSet<Moto> Motos { get; set; }

        public MotoDB(DbContextOptions<MotoDB> options) : base(options)
        {
        }

    }
}