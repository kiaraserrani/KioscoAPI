using System;
using System.Collections.Generic;
using System.Text;
using Kiosco.Aplicacion.Common;
using Kiosco.Aplicacion.DTOs;
using Kiosco.Application.DTOs;
using Kiosco.Application.Interfaces;
using Kiosco.Domain.Entities;

namespace Kiosco.Application.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _repository;

        public ProductoService(IProductoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductoDto>> ObtenerTodosAsync()
        {
            var productos = await _repository.ObtenerTodosAsync();

            return productos.Select(p => new ProductoDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                Categoria = p.Categoria,
                Activo = p.Activo
            });
        }

       


        public async Task<Result<ProductoDto>> ObtenerPorIdAsync(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return Result<ProductoDto>.Failure(
                    "Producto no encontrado."
                );
            }

            var productoDto = new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                Categoria = producto.Categoria,
                Activo = producto.Activo
            };

            return Result<ProductoDto>.Success(productoDto);
        }

        public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            if (dto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                Categoria = dto.Categoria,
                Activo = true
            };

            var creado = await _repository.CrearAsync(producto);

            return new ProductoDto
            {
                Id = creado.Id,
                Nombre = creado.Nombre,
                Descripcion = creado.Descripcion,
                Precio = creado.Precio,
                Stock = creado.Stock,
                Categoria = creado.Categoria,
                Activo = creado.Activo
            };
        }

        public async Task<bool> ActualizarAsync(
            int id, CrearProductoDto dto)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);

            if (producto == null)
                return false;

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");

            if (dto.Precio <= 0)
                throw new ArgumentException("El precio debe ser mayor a cero.");

            if (dto.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo.");

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.Categoria = dto.Categoria;

            await _repository.ActualizarAsync(producto);

            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await _repository.ObtenerPorIdAsync(id);

            if (producto == null)
                return false;

            await _repository.EliminarAsync(producto);

            return true;
        }

    }
}
