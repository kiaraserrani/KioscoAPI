using Kiosco.Application.Interfaces;
using Kiosco.Domain.Entities;
using Kiosco.Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Kiosco.Infraestructura.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos
        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos.ToListAsync();
        }

        // Obtener un producto por ID
        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        // Crear un producto
        public async Task<Producto> CrearAsync(Producto producto)
        {
            await _context.Productos.AddAsync(producto);

            await _context.SaveChangesAsync();

            return producto;
        }

        // Actualizar un producto
        public async Task ActualizarAsync(Producto producto)
        {
            _context.Productos.Update(producto);

            await _context.SaveChangesAsync();
        }

        // Eliminar un producto
        public async Task EliminarAsync(Producto producto)
        {
            _context.Productos.Remove(producto);

            await _context.SaveChangesAsync();
        }
    }
}
