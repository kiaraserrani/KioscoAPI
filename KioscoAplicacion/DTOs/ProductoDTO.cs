using System;
using System.Collections.Generic;
using System.Text;


namespace Kiosco.Application.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string Categoria { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}

