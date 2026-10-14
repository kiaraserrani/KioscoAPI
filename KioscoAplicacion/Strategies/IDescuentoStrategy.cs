using System;
using System.Collections.Generic;
using System.Text;

namespace Kiosco.Aplicacion.Strategies
{
    public interface IDescuentoStrategy
    {
        decimal CalcularPrecioFinal(decimal precioOriginal);
    }
}
