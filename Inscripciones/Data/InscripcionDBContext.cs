using Microsoft.EntityFrameworkCore;

namespace Inscripciones.Data
{
    public class InscripcionDBContext:DbContext
    {
        public InscripcionDBContext(DbContextOptions<InscripcionDBContext> options) : base(options)
        {
        }
        public DbSet<Models.Inscripcion> Inscripcion { get; set; }
    }
}
