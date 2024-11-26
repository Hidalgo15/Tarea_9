using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
public class Usuario
{
    [Key]
    public int Id { get; set; }
    public string NombreUsuario { get; set; }
    public string CorreoElectronico { get; set; }
    public string Contraseña { get; set; }

    public ICollection<Vivencia> Vivencias { get; set; }
}

public class Vivencia
{
    [Key]
    public int Id { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateOnly Fecha { get; set; }
    public string Imagen { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}