using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints;

public static class RolEndpoints
{
    public static RouteGroupBuilder MapRolEndpoints(this RouteGroupBuilder app)
    {
        List<Rol> roles = new List<Rol>
        {
            new Rol { IdRol = 1, Nombre = "Admin", Habilitado = true, FechaCreacion = DateTime.Now },
            new Rol { IdRol = 2, Nombre = "Usuario", Habilitado = true, FechaCreacion = DateTime.Now }
        };
        
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
    }
}