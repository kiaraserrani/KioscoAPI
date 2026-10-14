using System;
using System.Collections.Generic;
using System.Text;

using Kiosco.Aplicacion.Common;
using Kiosco.Aplicacion.Strategies;

namespace Kiosco.Application.Services
{
    public class DescuentoService
    {
        public Result<decimal> CalcularPrecioFinal(
            decimal precioOriginal,
            string tipoDescuento)
        {
            if (precioOriginal <= 0)
            {
                return Result<decimal>.Failure(
                    "El precio debe ser mayor a cero."
                );
            }

            if (string.IsNullOrWhiteSpace(tipoDescuento))
            {
                return Result<decimal>.Failure(
                    "Debe indicar un tipo de descuento."
                );
            }

            IDescuentoStrategy estrategia;

            switch (tipoDescuento.Trim().ToLowerInvariant())
            {
                case "ninguno":
                    estrategia = new SinDescuentoStrategy();
                    break;

                case "promocion":
                    estrategia = new Descuento10Strategy();
                    break;

                case "especial":
                    estrategia = new Descuento20Strategy();
                    break;

                default:
                    return Result<decimal>.Failure(
                        "El tipo de descuento no es válido."
                    );
            }

            decimal precioFinal =
                estrategia.CalcularPrecioFinal(precioOriginal);

            precioFinal = Math.Round(
                precioFinal,
                2,
                MidpointRounding.AwayFromZero
            );

            return Result<decimal>.Success(precioFinal);
        }
    }
}

