using Inscripciones.Data;
using Inscripciones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Inscripciones.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionController:ControllerBase 
    {

        private readonly InscripcionDBContext _dbContext;
        private readonly HttpClient _httpClient;


        public InscripcionController(InscripcionDBContext inscripcionDBContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = inscripcionDBContext;
            _httpClient = httpClientFactory.CreateClient("CursosAPI");
        }

        [HttpPost]
        public async Task<IActionResult> Inscribirse(int cursoId, string empleadoId)
        {
            var curso = await _httpClient.GetFromJsonAsync<CursoDto>($"api/Curso/{cursoId}");
            if (curso == null)
                return NotFound("Curso no encontrado");

            var inscripcion = new Inscripcion
            {
                CursoId = cursoId,
                EmpleadoId = empleadoId,
                FechaInscripcion = DateTime.UtcNow
            };

            _dbContext.Inscripcion.Add(inscripcion);
            await _dbContext.SaveChangesAsync();

            return Ok("Inscripción registrada");
        }

    }
}
