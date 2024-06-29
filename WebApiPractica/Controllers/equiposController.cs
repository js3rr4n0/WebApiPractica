using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WebApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : Controller
    {

        private readonly equiposContext _equiposContexto;



        public ClientesController(equiposContext equiposContexto)
        {
            _equiposContexto = equiposContexto;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<equipos>>> GetClientes()
        {
            return await _equiposContexto.equipos.ToListAsync();
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<equipos>> GetCliente(int id)
        {
            var cliente = await _equiposContexto.equipos.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // POST: api/Clientes
        [HttpPost]
        public async Task<ActionResult<equipos>> PostCliente(equipos cliente)
        {
            _equiposContexto.equipos.Add(cliente);
            await _equiposContexto.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.id_equipos }, cliente);
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarEquipo(int id, [FromBody] equipos equipoModificar)
        {
            // Para actualizar un registro, se obtiene el registro original de la base de datos
            // al cual alteraremos alguna propiedad
            var equipoActual = await _equiposContexto.equipos.FirstOrDefaultAsync(e => e.id_equipos == id);

            // Verificamos que exista el registro según su ID
            if (equipoActual == null)
            {
                return NotFound();
            }

            // Si se encuentra el registro, se alteran los campos modificables
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.descripcion = equipoModificar.descripcion;
            equipoActual.marca_id = equipoModificar.marca_id;
            equipoActual.modelo = equipoModificar.modelo;
            equipoActual.tipo_equipo_id = equipoModificar.tipo_equipo_id;
            equipoActual.anio_compra = equipoModificar.anio_compra;
            equipoActual.costo = equipoModificar.costo;

            // Se marca el registro como modificado en el contexto
            // y se envía la modificación a la base de datos
            _equiposContexto.Entry(equipoActual).State = EntityState.Modified;

            try
            {
                await _equiposContexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(equipoModificar);
        }

        // Método para verificar si el equipo existe
        private bool EquipoExists(int id)
        {
            return _equiposContexto.equipos.Any(e => e.id_equipos == id);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _equiposContexto.equipos.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _equiposContexto.equipos.Remove(cliente);
            await _equiposContexto.SaveChangesAsync();

            return NoContent();
        }

        private bool ClienteExists(int id)
        {
            return _equiposContexto.equipos.Any(e => e.id_equipos == id);
        }
    }
}