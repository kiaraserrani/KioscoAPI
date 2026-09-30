using MediatR;
using Kiosco.Aplicacion.Common;
using Kiosco.Application.DTOs;
using Kiosco.Application.Services;

namespace Kiosco.Aplicacion.Features.Productos.Queries
{
    public class ObtenerProductoPorIdHandler
        : IRequestHandler<ObtenerProductoPorIdQuery, Result<ProductoDto>>
    {
        private readonly ProductoService _productoService;

        public ObtenerProductoPorIdHandler(
            ProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<Result<ProductoDto>> Handle(
            ObtenerProductoPorIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _productoService.ObtenerPorIdAsync(request.Id);
        }
    }
}
