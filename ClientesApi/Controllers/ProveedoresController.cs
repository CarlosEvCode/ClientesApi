using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProveedoresController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedor>>> Listar([FromQuery] string? buscar)
        {
            var consulta = _context.Proveedores.AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var texto = buscar.Trim().ToLower();
                consulta = consulta.Where(p =>
                    p.RazonSocial.Contains(texto) ||
                    p.Ruc.Contains(texto) ||
                    p.Correo.Contains(texto));
            }

            var lista = await consulta
                .OrderBy(p => p.RazonSocial)
                .Take(200).ToListAsync();
            return Ok(lista);
        }

        //GET: api/Proveedores/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Proveedor>> ObtenerPorId(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound(new { message = $"No existe el proveedor con ID: {id}" });
            }
            return Ok(proveedor);
        }

        //POST: api/Proveedores
        [HttpPost]
        public async Task<ActionResult<Proveedor>> Registrar([FromBody] Proveedor proveedor)
        {
            proveedor.Id = 0;

            //El RUC es único
            if (await _context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc))
            {
                return Conflict(new { mensaje = "El RUC ya está registrado" });
            }

            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
        }

        //PUT: api/Proveedores/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Proveedor proveedor)
        {
            if (id != proveedor.Id)
            {
                return BadRequest(new { message = "El ID del proveedor no coincide" });
            }

            var existente = await _context.Proveedores.FindAsync(id);

            if (existente == null)
            {
                return NotFound(new { message = $"No existe el proveedor con ID: {id}" });
            }

            //No permitir RUC repetido en otro registro
            if (await _context.Proveedores.AnyAsync(p => p.Ruc == proveedor.Ruc && p.Id != id))
            {
                return Conflict(new { message = "Ya existe un proveedor con ese RUC" });
            }

            existente.RazonSocial = proveedor.RazonSocial;
            existente.Ruc = proveedor.Ruc;
            existente.Telefono = proveedor.Telefono;
            existente.Direccion = proveedor.Direccion;
            existente.Correo = proveedor.Correo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE: api/Proveedores/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);

            if (proveedor == null)
            {
                return NotFound(new { message = $"No existe el proveedor con ID: {id}" });
            }

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
