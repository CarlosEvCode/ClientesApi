# Clientes y Productos API

API RESTful desarrollada en .NET 10 con Entity Framework Core y SQL Server para la gestión de clientes y productos.

---

## Base de Datos (DemoClientes)

Tablas utilizadas por la solución:

### Tabla clientes

```sql
CREATE TABLE clientes
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    apellidos   NVARCHAR(70)     NOT NULL,
    nombres     NVARCHAR(70)     NOT NULL,
    telefono    NVARCHAR(9)      NOT NULL,
    correo      NVARCHAR(150)    NOT NULL,
    dni         CHAR(8)          NOT NULL,
    direccion   NVARCHAR(150)    NOT NULL,
    CONSTRAINT uk_dni_cli UNIQUE (dni),
    CONSTRAINT ck_dni_cli CHECK (dni LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
);
GO
```

### Tabla productos

```sql
CREATE TABLE productos
(
    id          INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(100)    NOT NULL,
    descripcion NVARCHAR(100)    NOT NULL,
    precio      DECIMAL(7,2)     NOT NULL,
    garantia    TINYINT          NOT NULL,
    numserie    NVARCHAR(40)     NULL
);
GO
```

---

## Cadena de Conexión (appsettings.json)

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,14333;Database=DemoClientes;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
}
```

---

## Endpoints

### Clientes (`/api/clientes`)
- `GET /api/clientes` - Listar clientes
- `GET /api/clientes/{id}` - Buscar cliente por ID
- `POST /api/clientes` - Crear cliente
- `PUT /api/clientes/{id}` - Actualizar cliente
- `DELETE /api/clientes/{id}` - Eliminar cliente

### Productos (`/api/productos`)
- `GET /api/productos` - Listar productos
- `GET /api/productos/{id}` - Buscar producto por ID
- `POST /api/productos` - Crear producto
- `PUT /api/productos/{id}` - Actualizar producto
- `DELETE /api/productos/{id}` - Eliminar producto
