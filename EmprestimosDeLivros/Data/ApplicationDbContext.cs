using EmprestimosDeLivros.Models;
using Microsoft.EntityFrameworkCore;

namespace EmprestimosDeLivros.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<EmprestimosModel> Emprestimos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
