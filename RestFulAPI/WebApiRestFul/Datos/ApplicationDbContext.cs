using Microsoft.EntityFrameworkCore;
using WebApiRestFul.Modelos;

namespace WebApiRestFul.Datos
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }
        public DbSet<Country> Countries { get; set; }
    }
}
