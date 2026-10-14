
# KioscoAPI

API REST desarrollada en **ASP.NET Core con C#** para la gestión de productos de un kiosco.

El proyecto fue realizado como trabajo práctico de la materia **Metodologías y Desarrollo Web**, implementando una arquitectura por capas y utilizando **Entity Framework Core como ORM, MediatR y los patrones de diseño Repository, Result y Strategy**.

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

El controlador recibe las solicitudes HTTP, utiliza los servicios correspondientes y devuelve las respuestas HTTP.

### KioscoAplicacion

Contiene la lógica de aplicación.

Incluye:

- DTOs
- Interfaces
- Servicios
- Patrón Result
- Patrón Strategy
- Queries y Handlers utilizados por MediatR

Dentro de `Features/Productos/Queries` se encuentra la consulta para obtener un producto por su identificador mediante MediatR.

En la carpeta `Services` se encuentran:

- `ProductoService`: administra las operaciones relacionadas con los productos.
- `DescuentoService`: selecciona y ejecuta las estrategias de descuento.

En la carpeta `Strategies` se encuentran:

- `IDescuentoStrategy`
- `SinDescuentoStrategy`
- `Descuento10Strategy`
- `Descuento20Strategy`

Estas clases permiten aplicar distintas políticas de descuento sin incorporar los algoritmos directamente en el controlador.

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

La API permite realizar operaciones CRUD sobre los productos y calcular precios finales aplicando diferentes descuentos.

| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/Productos` | Obtiene todos los productos |
| GET | `/api/Productos/{id}` | Obtiene un producto por ID |
| POST | `/api/Productos` | Crea un nuevo producto |
| PUT | `/api/Productos/{id}` | Actualiza un producto |
| DELETE | `/api/Productos/{id}` | Elimina un producto |
| GET | `/api/Productos/{id}/precio-final` | Calcula el precio final aplicando una estrategia de descuento |

## ORM - Entity Framework Core

Se utiliza **Entity Framework Core** como ORM para realizar la comunicación entre la aplicación y SQL Server.

`ApplicationDbContext` representa el contexto de acceso a la base de datos.

El repositorio `ProductoRepository` utiliza este contexto para realizar las operaciones correspondientes sobre los productos.

El uso de Entity Framework Core permite trabajar con entidades de C# y realizar operaciones sobre la base de datos sin escribir manualmente las consultas SQL de cada operación.

## Patrón Repository

El acceso a datos se abstrae mediante la interfaz:

`IProductoRepository`

Su implementación se encuentra en:

`ProductoRepository`

Esto permite separar la lógica de aplicación de la implementación concreta utilizada para acceder a SQL Server.

Entre sus beneficios se encuentran:

- Separación de responsabilidades.
- Menor acoplamiento entre capas.
- Mayor facilidad de mantenimiento.
- Posibilidad de sustituir la implementación del acceso a datos.

## Patrón Mediator

Se utiliza **MediatR** para desacoplar el controlador del procesamiento de determinadas solicitudes.

La consulta de un producto por ID se realiza mediante:

`ObtenerProductoPorIdQuery`

Y es procesada por:

`ObtenerProductoPorIdHandler`

El controlador envía la solicitud mediante `ISender` y MediatR se encarga de localizar el Handler correspondiente.

El flujo utilizado es:

`ProductosController -> MediatR -> Query -> Handler -> ProductoService -> Repository -> Entity Framework Core -> SQL Server`

Este patrón permite separar el envío de solicitudes de su procesamiento.

## Patrón Result

Se implementó una clase genérica:

`Result<T>`

Este patrón permite representar explícitamente si una operación fue exitosa o fallida.

La clase contiene las siguientes propiedades:

- `IsSuccess`: indica si la operación fue exitosa.
- `Value`: contiene el valor obtenido cuando la operación es exitosa.
- `Error`: contiene el mensaje correspondiente cuando la operación falla.

También utiliza los métodos:

- `Success()`: genera un resultado exitoso.
- `Failure()`: genera un resultado fallido.

### Aplicación en el negocio

El patrón Result se utiliza para:

- Consultar un producto por su identificador.
- Detectar cuando un producto no existe.
- Validar el tipo de descuento solicitado.
- Devolver el precio final calculado.

Cuando un producto existe, se devuelve un resultado exitoso con sus datos.

Cuando no existe, se devuelve un resultado fallido con el mensaje:

`Producto no encontrado.`

El controlador interpreta el resultado y devuelve la respuesta HTTP correspondiente:

- `200 OK` si la operación es exitosa.
- `400 Bad Request` si el descuento solicitado es inválido.
- `404 Not Found` si el producto no existe.

El patrón Result permite manejar errores de negocio esperados sin necesidad de lanzar excepciones.

## Patrón Strategy

Se implementó el patrón de diseño **Strategy** para calcular descuentos sobre los precios de los productos.

Este patrón permite definir diferentes algoritmos, encapsularlos en clases independientes y seleccionar cuál utilizar según las necesidades del negocio.

### Interfaz IDescuentoStrategy

Todas las estrategias implementan la interfaz:

`IDescuentoStrategy`

Esta interfaz define el método:

`CalcularPrecioFinal(decimal precioOriginal)`

Cada estrategia implementa su propia lógica para calcular el precio final.

### Estrategias implementadas

| Estrategia | Funcionamiento |
|---|---|
| `SinDescuentoStrategy` | Mantiene el precio original |
| `Descuento10Strategy` | Aplica un descuento del 10 % |
| `Descuento20Strategy` | Aplica un descuento del 20 % |

### DescuentoService

La clase `DescuentoService` se encarga de seleccionar la estrategia correspondiente según el tipo de descuento solicitado.

Los tipos disponibles son:

- `ninguno`: no aplica descuento.
- `promocion`: aplica un descuento del 10 %.
- `especial`: aplica un descuento del 20 %.

El servicio valida el precio original y el tipo de descuento solicitado.

Si los datos son válidos, selecciona la estrategia, calcula el precio final y devuelve un `Result<decimal>` exitoso.

Si ocurre un error de validación, devuelve un resultado fallido con el mensaje correspondiente.

### Beneficios del patrón Strategy

- Separa los algoritmos de descuento de la lógica del controlador.
- Permite incorporar nuevas estrategias de descuento.
- Favorece la reutilización del código.
- Facilita el mantenimiento.
- Reduce el acoplamiento entre componentes.
- Permite seleccionar distintos comportamientos según las necesidades del negocio.

## Integración de los patrones Result y Strategy

Para integrar ambos patrones se incorporó el endpoint:

`GET /api/Productos/{id}/precio-final`

Este endpoint permite consultar un producto almacenado en SQL Server y calcular su precio final aplicando una estrategia de descuento.

### Parámetros

- `id`: identificador del producto.
- `tipoDescuento`: tipo de descuento solicitado.

Los valores admitidos para `tipoDescuento` son:

- `ninguno`
- `promocion`
- `especial`

### Ejemplo de solicitud

`GET /api/Productos/1/precio-final?tipoDescuento=promocion`

### Ejemplo de respuesta exitosa

```json
{
  "productoId": 1,
  "nombre": "Coca-Cola 500ml",
  "precioOriginal": 2500,
  "tipoDescuento": "promocion",
  "precioFinal": 2250
}
```

En este ejemplo, el producto tiene un precio original de $2.500.

La estrategia `Descuento10Strategy` aplica un descuento del 10 %:

`2500 * 0.90 = 2250`

Por lo tanto, el precio final es de $2.250.

### Funcionamiento interno

El proceso se desarrolla de la siguiente manera:

1. El cliente realiza una solicitud HTTP al endpoint.
2. `ProductosController` recibe el identificador y el tipo de descuento.
3. El controlador utiliza MediatR para consultar el producto.
4. El Handler obtiene el producto mediante los servicios y el repositorio.
5. Entity Framework Core consulta los datos en SQL Server.
6. El patrón Result permite comprobar si el producto existe.
7. Si existe, el controlador solicita el cálculo a `DescuentoService`.
8. `DescuentoService` selecciona la estrategia correspondiente.
9. La estrategia calcula el precio final.
10. El servicio devuelve un `Result<decimal>`.
11. El controlador interpreta el resultado y genera la respuesta HTTP.

De esta manera, los patrones Result y Strategy trabajan conjuntamente dentro de una funcionalidad real del negocio.

## Manejo de respuestas HTTP

| Código HTTP | Descripción |
|---|---|
| `200 OK` | Producto encontrado y descuento calculado correctamente |
| `400 Bad Request` | Tipo de descuento inválido |
| `404 Not Found` | Producto inexistente |
| `500 Internal Server Error` | Error inesperado de la aplicación |

## Middleware de excepciones

La API posee un middleware denominado:

`ExceptionMiddleware`

Su función es centralizar el manejo de errores inesperados de la aplicación.

Ante una excepción no controlada, la API devuelve una respuesta HTTP:

`500 Internal Server Error`

El middleware se diferencia del patrón Result porque se utiliza para gestionar excepciones inesperadas, mientras que Result permite representar errores de negocio esperados.

## Base de datos

El proyecto utiliza **SQL Server**.

Dentro de la carpeta:

`SQL`

Se encuentra el archivo:

`KioscoDB.sql`

Este script permite:

- Crear la base de datos `KioscoDB`.
- Crear la tabla `Productos`.
- Incorporar un producto inicial para realizar pruebas.

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
```

La configuración debe adaptarse al servidor SQL Server utilizado en cada entorno.

## Pruebas mediante Swagger

La API utiliza Swagger / OpenAPI para documentar y probar sus endpoints.

Una vez ejecutado el proyecto, se puede acceder a Swagger mediante la dirección HTTPS local configurada en Visual Studio.

### Pruebas de descuentos

Se pueden realizar las siguientes pruebas sobre el endpoint:

`GET /api/Productos/{id}/precio-final`

| ID | Tipo de descuento | Resultado esperado |
|---|---|---|
| 1 | `ninguno` | Precio original |
| 1 | `promocion` | Descuento del 10 % |
| 1 | `especial` | Descuento del 20 % |
| 1 | `invalido` | `400 Bad Request` |
| 999999 | `promocion` | `404 Not Found`, si el producto no existe |

Para un producto con precio original de $2.500:

- Sin descuento: $2.500.
- Promoción del 10 %: $2.250.
- Descuento especial del 20 %: $2.000.

Estas pruebas permiten comprobar la selección de estrategias y el manejo de resultados exitosos y fallidos.

## Beneficios de la implementación

La incorporación de los patrones de diseño permite mejorar la estructura y organización de la API.

### Result

- Centraliza la representación de éxitos y errores.
- Facilita el manejo de respuestas desde los controladores.
- Evita utilizar excepciones para errores de negocio esperados.
- Permite expresar claramente el resultado de una operación.

### Strategy

- Encapsula los algoritmos de descuento en clases independientes.
- Permite seleccionar diferentes comportamientos.
- Facilita la incorporación de nuevas estrategias.
- Favorece el mantenimiento y la reutilización del código.

### Arquitectura por capas

- Separa la presentación de la lógica de aplicación.
- Aísla el acceso a datos.
- Facilita el mantenimiento.
- Mejora la organización del proyecto.

## Conclusión

El proyecto KioscoAPI implementa una API REST para gestionar productos de un kiosco utilizando ASP.NET Core, Entity Framework Core y SQL Server.

En la primera entrega se desarrollaron las operaciones CRUD, la arquitectura por capas, el acceso a datos mediante Repository, la comunicación mediante MediatR y el manejo de resultados mediante Result.

En la segunda entrega se amplió la funcionalidad incorporando el patrón Strategy para calcular descuentos y se reutilizó el patrón Result para validar las operaciones y comunicar sus resultados.

La integración de ambos patrones permite resolver una necesidad concreta del negocio, manteniendo una separación clara de responsabilidades y facilitando futuras ampliaciones del sistema.
