
using MediatR;
using Kiosco.Aplicacion.Common;
using Kiosco.Application.DTOs;

namespace Kiosco.Aplicacion.Features.Productos.Queries
{
    public record ObtenerProductoPorIdQuery(int Id)
        : IRequest<Result<ProductoDto>>;
}
