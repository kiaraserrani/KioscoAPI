using System;
using System.Collections.Generic;
using System.Text;

namespace Kiosco.Aplicacion.Strategies
{
    public class SinDescuentoStrategy : IDescuentoStrategy
    {
        public decimal CalcularPrecioFinal(decimal precioOriginal)
        {
            return precioOriginal;
        }
    }
}
