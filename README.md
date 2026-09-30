# KioscoAPI

API REST desarrollada en **ASP.NET Core con C#** para la gestión de productos de un kiosco.

El proyecto fue realizado como trabajo práctico de la materia **Metodologías y Desarrollo Web**, implementando una arquitectura por capas y utilizando **Entity Framework Core como ORM, MediatR y el patrón Result**.

## Tecnologías utilizadas

- C#
- ASP.NET Core (.NET 10)
- SQL Server
- Entity Framework Core
- MediatR
- Swagger / OpenAPI
- Visual Studio 2022

## Arquitectura del proyecto

La solución está organizada en cuatro proyectos:

### Kiosco.API

Capa de presentación de la aplicación.

Contiene:

- `ProductosController`
- `ExceptionMiddleware`
- Configuración de servicios e inyección de dependencias en `Program.cs`
- Configuración de Swagger
- Configuración de conexión mediante `appsettings.json`

### KioscoAplicacion

Contiene la lógica de aplicación.

Incluye:

- DTOs
- Interfaces
- Servicios
- Patrón Result
- Queries y Handlers utilizados por MediatR

Dentro de `Features/Productos/Queries` se encuentra la consulta para obtener un producto por su identificador mediante Mediator.

### Kiosco.Domain

Contiene las entidades principales del dominio.

Actualmente se utiliza la entidad:

`Producto`

La entidad contiene:

- Id
- Nombre
- Descripción
- Precio
- Stock
- Categoría
- Activo

### Kiosco.Infraestructura

Contiene la implementación del acceso a datos.

Incluye:

- `ApplicationDbContext`
- `ProductoRepository`

La comunicación con SQL Server se realiza utilizando **Entity Framework Core**.

## Funcionalidades

La API permite realizar operaciones CRUD sobre los productos.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/Productos` | Obtiene todos los productos |
| GET | `/api/Productos/{id}` | Obtiene un producto por ID |
| POST | `/api/Productos` | Crea un nuevo producto |
| PUT | `/api/Productos/{id}` | Actualiza un producto |
| DELETE | `/api/Productos/{id}` | Elimina un producto |

## ORM - Entity Framework Core

Se utiliza **Entity Framework Core** como ORM para realizar la comunicación entre la aplicación y SQL Server.

`ApplicationDbContext` representa el contexto de acceso a la base de datos.

El repositorio `ProductoRepository` utiliza este contexto para realizar las operaciones correspondientes sobre los productos.

## Patrón Repository

El acceso a datos se abstrae mediante:

`IProductoRepository`

Su implementación se encuentra en:

`ProductoRepository`

Esto permite separar la lógica de aplicación de la implementación concreta utilizada para acceder a SQL Server.

## Patrón Mediator

Se utiliza **MediatR** para desacoplar el controlador del procesamiento de determinadas solicitudes.

La consulta de un producto por ID se realiza mediante:

`ObtenerProductoPorIdQuery`

y es procesada por:

`ObtenerProductoPorIdHandler`

El controlador envía la solicitud mediante `ISender` y MediatR se encarga de localizar el Handler correspondiente.

El flujo utilizado es:

`ProductosController -> MediatR -> Query -> Handler -> ProductoService -> Repository -> Entity Framework Core -> SQL Server`

## Patrón Result

Se implementó una clase genérica:

`Result<T>`

Este patrón permite representar explícitamente si una operación fue exitosa o fallida.

Se utiliza en la consulta de productos por ID.

Si el producto existe, se devuelve un resultado exitoso con el producto.

Si el producto no existe, se devuelve un resultado fallido con el mensaje:

`Producto no encontrado.`

El controlador interpreta el resultado y devuelve la respuesta HTTP correspondiente.

- `200 OK` si el producto existe.
- `404 Not Found` si el producto no existe.

## Middleware de excepciones

La API posee un middleware denominado:

`ExceptionMiddleware`

Su función es centralizar el manejo de errores inesperados de la aplicación.

Ante una excepción no controlada, la API devuelve una respuesta HTTP `500 Internal Server Error`.

## Base de datos

El proyecto utiliza **SQL Server**.

Dentro de la carpeta:

`SQL`

se encuentra:

`KioscoDB.sql`

Este script permite crear la base de datos `KioscoDB`, crear la tabla `Productos` y cargar un producto inicial para realizar pruebas.

### Configuración de conexión

La cadena de conexión se encuentra en:

`Kiosco.API/appsettings.json`

La configuración utilizada durante el desarrollo es:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=KioscoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}