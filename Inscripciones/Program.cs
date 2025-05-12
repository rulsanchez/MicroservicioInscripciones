using Inscripciones.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<InscripcionDBContext>(
    options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


//pregunta, entender los parametros de este tipo de metodos, donde hay una lambda dentro)
builder.Services.AddHttpClient("CursosAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5072"); // el puerto real de Cursos.API
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
