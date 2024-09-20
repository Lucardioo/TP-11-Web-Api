namespace Api;

public class Rol
{
    public int IdRol { get; set; }
    public required string Nombre { get; set;}
    public bool Habilitado { get; set;}
    public DateTime FechaCreacion { get; set;}
    public List<Rol> roles { get; set; } = new List<Rol>();
}
