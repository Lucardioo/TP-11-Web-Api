using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this RouteGroupBuilder app)
    {
        List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { IdUsuario = 1, Nombre = "Lucas", Email = "lucas@example.com", NombreUsuario = "lucas123", Contraseña = "pass123", Habilitado = true, FechaCreacion = DateTime.Now },
            new Usuario { IdUsuario = 2, Nombre = "Nahuel", Email = "nahuel@example.com", NombreUsuario = "nahuel123", Contraseña = "pass123", Habilitado = true, FechaCreacion = DateTime.Now }
        };

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
    }
}