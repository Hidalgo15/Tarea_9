using Microsoft.EntityFrameworkCore;

public class VicenciaContext: DbContext{

    public DbSet<Usuario> Usuarios {get; set;} = null;
    public DbSet<Vivencia> Vivencias {get; set;} = null;

    public VicenciaContext(DbContextOptions<VicenciaContext> options)
    : base(options) {}
}