using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
public class VivenciaService
{
    private readonly VicenciaContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public VivenciaService(VicenciaContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    private int GetCurrentUserId()
    {
        // Obtener el ID del usuario actual desde el contexto de HTTP
        // Ajusta esta lógica según tu implementación de autenticación
        var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return int.Parse(userIdClaim.Value);

    }

    public async Task<List<Vivencia>> GetVivenciasByUserId()
    {
        var userId = GetCurrentUserId();
        return await _context.Vivencias.Where(v => v.UsuarioId == userId).ToListAsync();
    }

    public async Task<Vivencia> GetVivenciaById(int vivenciaId)
    {
        var userId = GetCurrentUserId();
        return await _context.Vivencias.FirstOrDefaultAsync(v => v.Id == vivenciaId && v.UsuarioId == userId);
    }

    public async Task CreateVivencia(Vivencia vivencia)
    {
        vivencia.UsuarioId = GetCurrentUserId();
        _context.Vivencias.Add(vivencia);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateVivencia(Vivencia vivencia)
    {
        var existingVivencia = await _context.Vivencias.FindAsync(vivencia.Id);
        if (existingVivencia == null || existingVivencia.UsuarioId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("No tienes autorización para modificar esta vivencia.");
        }

        _context.Entry(vivencia).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVivencia(int vivenciaId)
    {
        var vivencia = await _context.Vivencias.FindAsync(vivenciaId);
        if (vivencia == null || vivencia.UsuarioId != GetCurrentUserId())
        {
            throw new UnauthorizedAccessException("No tienes autorización para eliminar esta vivencia.");
        }

        _context.Vivencias.Remove(vivencia);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAllVivenciasByUserId(int userId)
    {
        var vivencias = await _context.Vivencias.Where(v => v.UsuarioId == userId).ToListAsync();
        _context.Vivencias.RemoveRange(vivencias);
        await _context.SaveChangesAsync();
    }
}