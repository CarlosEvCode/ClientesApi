-- Base de datos
CREATE DATABASE DemoClientes;
GO

USE DemoClientes;
GO

-- Clientes
CREATE TABLE clientes
(
    id        INT IDENTITY(1,1) PRIMARY KEY,
    apellidos NVARCHAR(70)  NOT NULL,
    nombres   NVARCHAR(70)  NOT NULL,
    telefono  NVARCHAR(9)   NOT NULL,
    correo    NVARCHAR(150) NOT NULL,
    dni       CHAR(8)       NOT NULL UNIQUE,
    direccion NVARCHAR(150) NOT NULL,
    CHECK (dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);
GO

-- Productos
CREATE TABLE productos
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(100) NOT NULL,
    descripcion NVARCHAR(100) NOT NULL,
    precio      DECIMAL(7,2)  NOT NULL,
    garantia    TINYINT       NOT NULL,
    numserie    NVARCHAR(40)  NULL
);
GO

-- Proveedores
CREATE TABLE proveedores
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    razonsocial NVARCHAR(100) NOT NULL,
    ruc         CHAR(11)      NOT NULL UNIQUE,
    telefono    NVARCHAR(9)   NOT NULL,
    direccion   NVARCHAR(150) NOT NULL,
    correo      NVARCHAR(150) NOT NULL,
    CHECK (ruc LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);
GO

-- Usuarios (login)
CREATE TABLE Usuario
(
    id           INT IDENTITY(1,1) PRIMARY KEY,
    username     NVARCHAR(50)  NOT NULL UNIQUE,
    passwordhash NVARCHAR(200) NOT NULL,
    rol          NVARCHAR(30)  NOT NULL
);
GO

-- Verificación
SELECT * FROM clientes;
SELECT * FROM productos;
SELECT * FROM proveedores;
SELECT * FROM Usuario;
GO
