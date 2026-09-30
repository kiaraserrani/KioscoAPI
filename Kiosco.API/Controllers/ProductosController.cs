
using Kiosco.Aplicacion.DTOs;
using Kiosco.Application.DTOs;
using Kiosco.Application.Services;
using Kiosco.Aplicacion.Features.Productos.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Kiosco.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;
        private readonly ISender _sender;

        public ProductosController(
            ProductoService productoService,
            ISender sender)
        {
            _productoService = productoService;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var productos = await _productoService.ObtenerTodosAsync();

            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _sender.Send(
                new ObtenerProductoPorIdQuery(id)
            );

            if (!resultado.IsSuccess)
            {
                return NotFound(resultado.Error);
            }

            return Ok(resultado.Value);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(
            [FromBody] CrearProductoDto dto)
        {
            try
            {
                var producto = await _productoService.CrearAsync(dto);

                return CreatedAtAction(
                    nameof(ObtenerPorId),
                    new { id = producto.Id },
                    producto
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(
            [FromRoute] int id,
            [FromBody] CrearProductoDto dto)
        {
            try
            {
                var actualizado =
                    await _productoService.ActualizarAsync(id, dto);

                if (!actualizado)
                    return NotFound("Producto no encontrado.");

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _productoService.EliminarAsync(id);

            if (!eliminado)
                return NotFound("Producto no encontrado.");

            return NoContent();
        }
    }
}
