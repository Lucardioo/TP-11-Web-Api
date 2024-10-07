using Api;
using Api.Endpoints;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


List<Rol> roles = new List<Rol>
{
    new Rol { IdRol = 1, Nombre = "Admin", Habilitado = true, FechaCreacion = DateTime.Now },
    new Rol { IdRol = 2, Nombre = "Usuario", Habilitado = true, FechaCreacion = DateTime.Now }
};

List<Usuario> usuarios = new List<Usuario>
{
    new Usuario { IdUsuario = 1, Nombre = "Lucas", Email = "lucas@example.com", NombreUsuario = "lucas123", Contraseña = "pass123", Habilitado = true, FechaCreacion = DateTime.Now },
    new Usuario { IdUsuario = 2, Nombre = "Nahuel", Email = "nahuel@example.com", NombreUsuario = "nahuel123", Contraseña = "pass123", Habilitado = true, FechaCreacion = DateTime.Now }
};

app.MapGroup("/api")
    .MapUsuarioEndpoints()
    .WithTags("Usuario");

app.MapGroup("/api")
    .MapRolEndpoints()
    .WithTags("Rol");
    

app.Run();