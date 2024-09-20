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
//aca arranca


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

// Lee listado de roles
app.MapGet("/rol", () =>
{
    return Results.Ok(roles);
})
.WithTags("Rol");

// Crea un nuevo rol en la lista
app.MapPost("/rol", ([FromBody] Rol rol) =>
{
    if (string.IsNullOrEmpty(rol.Nombre))
    {
        return Results.BadRequest("El nombre del rol es requerido.");
    }

    rol.IdRol = roles.Count > 0 ? roles.Max(r => r.IdRol) + 1 : 1; // Genera un nuevo IdRol
    rol.FechaCreacion = DateTime.Now; // Asigna la fecha de creación
    roles.Add(rol);
    return Results.Created($"/rol/{rol.IdRol}", rol); // Devuelve 201 Created con el rol creado
})
.WithTags("Rol");

// Borra un rol en la lista
app.MapDelete("/rol", ([FromQuery] int idRol) =>
{
    var rolAEliminar = roles.FirstOrDefault(rol => rol.IdRol == idRol);
    if (rolAEliminar != null)
    {
        roles.Remove(rolAEliminar);
        return Results.Ok(roles); // Código 200
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Rol");

// Actualiza un rol en la lista
app.MapPut("/rol", ([FromQuery] int idRol, [FromBody] Rol rol) =>
{
    var rolAActualizar = roles.FirstOrDefault(rol => rol.IdRol == idRol);
    if (rolAActualizar != null)
    {
        rolAActualizar.Nombre = rol.Nombre;
        rolAActualizar.Habilitado = rol.Habilitado;
        return Results.Ok(roles); // Código 200
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Rol");

// Lee listado de usuarios
app.MapGet("/usuario", () =>
{
    return Results.Ok(usuarios);
})
.WithTags("Usuario");

// Crea un nuevo usuario en la lista
app.MapPost("/usuario", ([FromBody] Usuario usuario) =>
{
    usuarios.Add(usuario);
    return Results.Ok(usuarios);
})
.WithTags("Usuario");

// Borra un usuario en la lista
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

// Actualiza un usuario en la lista
app.MapPut("/usuario", ([FromQuery] int idUsuario, [FromBody] Usuario usuario) =>
{
    var usuarioAActualizar = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == idUsuario);
    if (usuarioAActualizar != null)
    {
        usuarioAActualizar.Nombre = usuario.Nombre;
        usuarioAActualizar.Email = usuario.Email;
        usuarioAActualizar.NombreUsuario = usuario.NombreUsuario;
        usuarioAActualizar.Contraseña = usuario.Contraseña;
        usuarioAActualizar.Habilitado = usuario.Habilitado;
        return Results.Ok(usuarios); // Código 200
    }
    else
    {
        return Results.NotFound(); // Código 404
    }
})
.WithTags("Usuario");

// Asocia un usuario a un rol
app.MapPost("/usuario/{idUsuario}/rol/{idRol}", (int idUsuario, int idRol) =>
{
    var usuario = usuarios.FirstOrDefault(usuario => usuario.IdUsuario == idUsuario);
    var rol = roles.FirstOrDefault(rol => rol.IdRol == idRol);

    if (usuario != null && rol != null)
    {
        usuariosRoles.Add(new UsuarioRol { IdUsuarioRol = usuariosRoles.Count + 1, IdUsuario = idUsuario, IdRol = idRol });
        return Results.Ok();
    }

    return Results.NotFound();
})
.WithTags("UsuarioRol");

// Desasigna un rol de un usuario
app.MapDelete("/usuario/{idUsuario}/rol/{idRol}", (int idUsuario, int idRol) =>
{
    var usuarioRol = usuariosRoles.FirstOrDefault(ur => ur.IdUsuario == idUsuario && ur.IdRol == idRol);

    if (usuarioRol != null)
    {
        usuariosRoles.Remove(usuarioRol);
        return Results.Ok(); // Código 200
    }

    return Results.NotFound(); // Código 404
})
.WithTags("UsuarioRol");




//cambios hechoss
//aca termina
app.Run();