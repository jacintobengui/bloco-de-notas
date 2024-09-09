using EditorDeTexto.Models;
using Microsoft.EntityFrameworkCore;

namespace EditorDeTexto.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Documento> Documentos { get; set; }
    }
}
