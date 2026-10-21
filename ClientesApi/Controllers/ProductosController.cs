using ClientesApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Models;

namespace ProductosApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;
        //Constructor
        public ProductosController(AppDbContext context)
        {
            _context = context;
        }
        //GET: api/Productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> Listar()
        {
            var productos = await _context.Productos.AsNoTracking().ToListAsync();
            return Ok(productos);
        }
        //GET: api/Productos/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Producto>> ObtenerPorId(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = $"No existe el producto con ID: {id}" });
            }
            return Ok(producto);
        }
        //POST: api/Productos
        [HttpPost]
        public async Task<ActionResult<Producto>> Registrar([FromBody] Producto producto)
        {
            producto.Id = 0;
            if (await _context.Productos.AnyAsync(p => p.NumSerie == producto.NumSerie))
            {
                return Conflict(new { mensaje = "El número de serie ya está registrado" });
            }
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
        }

        //PUT: api/Productos/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest(new { message = "El ID del producto no coincide" });
            }

            var existente = await _context.Productos.FindAsync(id);

            if (existente == null)
            {
                return NotFound(new { message = $"No existe el producto con ID: {id}" });
            }
            if (await _context.Productos.AnyAsync(p => p.NumSerie == producto.NumSerie && p.Id != id))
            {
                return Conflict(new { message = "Ya existe un producto con ese número de serie" });
            }

            existente.Nombre = producto.Nombre;
            existente.Descripcion = producto.Descripcion;
            existente.Precio = producto.Precio;
            existente.Garantia = producto.Garantia;
            existente.NumSerie = producto.NumSerie;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        //DELETE: api/Productos/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = $"No existe el producto con ID: {id}" });
            }
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}