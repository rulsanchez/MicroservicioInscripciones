namespace Inscripciones.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        public int CursoId { get; set; }
        public string EmpleadoId { get; set; }  // o int si no hay autenticación
        public DateTime FechaInscripcion { get; set; }
    }
}
