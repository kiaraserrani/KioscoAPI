
CREATE DATABASE KioscoDB;
GO

USE KioscoDB;
GO

CREATE TABLE Productos
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    Nombre NVARCHAR(100) NOT NULL,

    Descripcion NVARCHAR(255) NOT NULL,

    Precio DECIMAL(18,2) NOT NULL,

    Stock INT NOT NULL,

    Categoria NVARCHAR(100) NOT NULL,

    Activo BIT NOT NULL DEFAULT 1,

    CONSTRAINT CK_Productos_Precio
        CHECK (Precio > 0),

    CONSTRAINT CK_Productos_Stock
        CHECK (Stock >= 0)
);
GO


INSERT INTO Productos
(
    Nombre,
    Descripcion,
    Precio,
    Stock,
    Categoria,
    Activo
)
VALUES
(
    'Coca-Cola 500ml',
    'Gaseosa individual',
    2500.00,
    30,
    'Bebidas',
    1
);
GO

SELECT * FROM Productos;