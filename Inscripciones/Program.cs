//using Inscripciones.Data;
//using Microsoft.EntityFrameworkCore;
//using Azure.Identity;
//var builder = WebApplication.CreateBuilder(args);


//// Obtener el nombre del Key Vault desde configuración o directamente aquí
//string keyVaultName = "RaulKey";
//var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
//// Add services to the container.

//builder.Services.AddDbContext<InscripcionDBContext>(
//    options=> options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddDbContext<InscripcionDBContext>(options =>
//    options.UseSqlServer(builder.Configuration["SecretoRaul"]));

////pregunta, entender los parametros de este tipo de metodos, donde hay una lambda dentro)
//builder.Services.AddHttpClient("CursosAPI", client =>
//{
//    client.BaseAddress = new Uri("http://localhost:5072"); // el puerto real de Cursos.API
//});

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


using Inscripciones.Data;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Cargar configuración desde JSON, entorno, y secretos locales
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(); // solo para desarrollo local

// Obtener la URL del Key Vault desde configuración (App Service o secrets)
var keyVaultEndpoint = builder.Configuration["KeyVault:Endpoint"];
if (string.IsNullOrWhiteSpace(keyVaultEndpoint))
    throw new InvalidOperationException("❌ La variable 'KeyVault__Endpoint' no está configurada.");

var keyVaultUri = new Uri(keyVaultEndpoint);

// Agregar configuración desde Azure Key Vault
builder.Configuration.AddAzureKeyVault(keyVaultUri, new DefaultAzureCredential());

// Leer la cadena de conexión desde Key Vault
var connectionString = builder.Configuration.GetConnectionString("CursosDB");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("❌ No se encontró la cadena de conexión 'CursosDB'.");

builder.Services.AddDbContext<InscripcionDBContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar HttpClient para llamar a Cursos API (ajusta la URL si lo subes a Azure)
builder.Services.AddHttpClient("CursosAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5072"); // 👈 cambia por la URL real si está en Azure
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
