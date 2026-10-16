using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        //Todas las funcionalidades GET - POST - PUT - DELETE necesitan acceso a BD (ORM)
        private readonly AppDbContext _context;

        //Constructor (inicializar)
        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        //GET: api/Clientes -> retornar la lista de clientes
        //async : asincrono (no bloquea el hilo de ejecución)
        //IEnumerable : lista de objetos (colección <Tipo>)
        //ActionResult : respuesta HTTP (200, 404, 500, etc)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
        {
            var clientes = await _context.Clientes.AsNoTracking().ToListAsync();
            return Ok(clientes);
        }

        //GET: api/Clientes/{id} -> retornar un cliente por id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                //Error 404
                return NotFound(new { message = $"No existe el cliente con ID: {id}" });
            }
            //Ok (metodo de respuesta HTTP 200)
            return Ok(cliente);
        }

        //POST: api/Clientes -> crear un cliente (no olvidar que debemos enviar en el Body un JSON {})
        [HttpPost]
        public async Task<ActionResult<Cliente>> Registrar([FromBody] Cliente cliente)
        {
            //Evitar que el cliente envie un ID manualmente
            cliente.Id = 0;

            if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni)) 
            {
                //Server Code 409
                return Conflict(new { mensaje = "El DNI ya está registrado" });
            }

            //Procedemos al registro
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            //Server Code 201 (Created) - Ejecutamos en segundo plano el buscador por ID, pasamos el ID entrada, modelo
            return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
        }

        //PUT: api/Clientes/{id} -> actualizar (lleva un json en el body)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente cliente)
        {
            //Validacion 1: Coincidencia de ID's
            if (id != cliente.Id)
            {
                //Server Code 400
                return BadRequest(new { message = "El ID del cliente no coincide" });
            }

            //Validacion 2: ¿Existe el ID?
            var existente = await _context.Clientes.FindAsync(id); //FindAsync busca por PK (Primary Key)

            if (existente == null)
            {
                //Server Code 404
                return NotFound(new { message = $"No existe el cliente con ID: {id}" });
            }

            //Validacion 3: Conflicto DNI repetido
            if(await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni && c.Id != id))
            {
                //Server Code 409
                return Conflict(new { message = "Ya existe un cliente con ese DNI" });
            }

            //Procedemos a realizar la actualización...
            existente.Apellidos = cliente.Apellidos;
            existente.Nombres = cliente.Nombres;
            existente.Telefono = cliente.Telefono;
            existente.Correo = cliente.Correo;
            existente.Dni = cliente.Dni;
            existente.Direccion = cliente.Direccion;

            //Ahora confirmemos los cambios utilizando el contenedor "existente"
            await _context.SaveChangesAsync(); //está enlazado a <<<FindAsync>>>

            //Server Code 204
            return NoContent();
        }

        //DELETE: api/Clientes/{id} -> eliminar un cliente por id
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            //Validacion: ¿Existe el ID?
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                //Server Code 404
                return NotFound(new { message = $"No existe el cliente con ID: {id}" });
            }

            //Procedemos a eliminar...
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            //Server Code 204
            return NoContent();
        }

    }
}
