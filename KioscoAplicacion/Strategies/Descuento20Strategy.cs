using System;
using System.Collections.Generic;
using System.Text;

namespace Kiosco.Aplicacion.Strategies
{
    public class Descuento20Strategy : IDescuentoStrategy
    {
        public decimal CalcularPrecioFinal(decimal precioOriginal)
        {
            return precioOriginal * 0.80m;
        }
    }
}
