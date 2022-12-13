using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace webAppHash.Models
{
    public class UsuariosContext : DbContext
    {
        public DbSet<Usuarios> Usuarios { get; set; }

        public UsuariosContext(DbContextOptions<UsuariosContext> options) : base(options)
        {
        }



    }
}
