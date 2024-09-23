using Api;
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

List<UsuarioRol> usuariosRoles = new List<UsuarioRol>();



//Usuarios

// POST - Crea un nuevo usuario en la lista
app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
{
    if (string.IsNullOrEmpty(usuario.Nombre) || string.IsNullOrEmpty(usuario.Email) ||
        string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.Contraseña))
    {
        return Results.BadRequest("Faltan datos del usuario");
    }

    usuario.IdUsuario = usuarios.Count > 0 ? usuarios.Max(u => u.IdUsuario) + 1 : 1; // Asignar un nuevo ID
    usuarios.Add(usuario);
    return Results.Created($"/usuario/{usuario.IdUsuario}", usuario);
})
.WithTags("Usuario");

// GET - Ver todos los datos de todos los usuarios
app.MapGet("/usuarios", () =>
{
    return Results.Ok(usuarios); // Devuelve 200 OK con la lista completa de usuarios
})
.WithTags("Usuario");

// GET - Ver el detalle del usuario especificado por el id
app.MapGet("/usuario/{id}", (int id) =>
{
    var usuario = usuarios.FirstOrDefault(u => u.IdUsuario == id);
    
    if (usuario == null)
    {
        return Results.NotFound(); // Código 404 si no existe el usuario
    }
    
    return Results.Ok(usuario); // Devuelve 200 OK con los detalles del usuario
})
.WithTags("Usuario");

// PUT - Modificar el contenido de un usuario
app.MapPut("/usuario", ([FromQuery] int idUsuario, [FromBody] Usuario usuario) =>
{
    var usuarioAActualizar = usuarios.FirstOrDefault(u => u.IdUsuario == idUsuario);
    
    if (usuarioAActualizar == null)
    {
        return Results.NotFound(); // Código 404
    }

    if (!string.IsNullOrEmpty(usuario.Nombre) && usuario.Nombre != usuarioAActualizar.Nombre)
    {
        return Results.BadRequest("El nombre no se puede modificar"); // Código 400
    }

    usuarioAActualizar.Email = usuario.Email;
    usuarioAActualizar.NombreUsuario = usuario.NombreUsuario;
    usuarioAActualizar.Contraseña = usuario.Contraseña;
    usuarioAActualizar.Habilitado = usuario.Habilitado;

    return Results.NoContent(); // Código 204
})
.WithTags("Usuario");

// DELETE - Borrar un usuario especificando un id
app.MapDelete("/usuario", ([FromQuery] int idUsuario) =>
{
    var usuarioAEliminar = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == idUsuario);
    if (usuarioAEliminar != null)
    {
        usuarios.Remove(usuarioAEliminar);
        return Results.Ok(usuarios); // Código 200
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Usuario");



//Roles

// POST - Crea un nuevo rol
app.MapPost("/rol", ([FromBody] Rol rol) =>
{
    if (string.IsNullOrEmpty(rol.Nombre))
    {
        return Results.BadRequest("El nombre del rol es requerido."); // Código 400
    }

    rol.IdRol = roles.Count > 0 ? roles.Max(r => r.IdRol) + 1 : 1; // Genera un nuevo IdRol
    rol.FechaCreacion = DateTime.Now; // Asigna la fecha de creación
    roles.Add(rol);
    return Results.Created($"/rol/{rol.IdRol}", rol); // Devuelve 201 Created
})
.WithTags("Rol");

// GET - Ver todos los datos de todos los roles
app.MapGet("/roles", () =>
{
    return Results.Ok(roles); // Devuelve 200 OK con la lista completa de roles
})
.WithTags("Rol");

// GET - Ver el detalle de un rol especificado por ID
app.MapGet("/rol/{id}", (int id) =>
{
    var rol = roles.FirstOrDefault(r => r.IdRol == id);
    
    if (rol == null)
    {
        return Results.NotFound(); // Código 404 si no existe el rol
    }
    
    return Results.Ok(rol); // Devuelve 200 OK con los detalles del rol
})
.WithTags("Rol");

// PUT - Modificar el contenido de un rol
app.MapPut("/rol", ([FromQuery] int idRol, [FromBody] Rol rol) =>
{
    var rolAActualizar = roles.FirstOrDefault(r => r.IdRol == idRol);
    
    // Verificar si el rol existe
    if (rolAActualizar == null)
    {
        return Results.NotFound(); // Código 404
    }

    // Verificar si se intenta modificar el nombre
    if (!string.IsNullOrEmpty(rol.Nombre) && rol.Nombre != rolAActualizar.Nombre)
    {
        return Results.BadRequest("El nombre del rol no se puede modificar"); // Código 400
    }

    // Actualizar los demás campos
    rolAActualizar.Habilitado = rol.Habilitado;

    return Results.NoContent(); // Código 204
})
.WithTags("Rol");

// DELETE - Borrar un rol especificando un id
app.MapDelete("/rol", ([FromQuery] int idRol) =>
{
    var rolAEliminar = roles.FirstOrDefault(r => r.IdRol == idRol);
    if (rolAEliminar != null)
    {
        roles.Remove(rolAEliminar);
        return Results.NoContent(); // Código 204
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Rol");





app.Run();