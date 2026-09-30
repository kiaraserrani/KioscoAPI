using System;
using System.Collections.Generic;
using System.Text;


using Kiosco.Domain.Entities;

namespace Kiosco.Application.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();

        Task<Producto?> ObtenerPorIdAsync(int id);

        Task<Producto> CrearAsync(Producto producto);

        Task ActualizarAsync(Producto producto);

        Task EliminarAsync(Producto producto);
    }
}
