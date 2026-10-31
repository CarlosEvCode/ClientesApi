# Clientes y Productos API

API RESTful desarrollada en .NET 10 con Entity Framework Core y SQL Server para la gestión de clientes, productos y proveedores.

---

## Base de Datos (DemoClientes)

El script está en [`ClientesApi/Database/DemoClientes.sql`](ClientesApi/Database/DemoClientes.sql) y crea todas las tablas de la API.

| Tabla | Columnas | Usada por |
|---|---|---|
| `clientes` | id, apellidos, nombres, telefono, correo, dni, direccion | `ClientesController` |
| `productos` | id, nombre, descripcion, precio, garantia, numserie | `ProductosController` |
| `proveedores` | id, razonsocial, ruc, telefono, direccion, correo | `ProveedoresController` |
| `Usuario` | id, username, passwordhash, rol | `AuthController` |

Restricciones:

- `clientes.dni`: `UNIQUE` + `CHECK` de 8 dígitos.
- `proveedores.ruc`: `UNIQUE` + `CHECK` de 11 dígitos.
- `Usuario.username`: `UNIQUE`. El `passwordhash` se genera con BCrypt desde `POST /api/auth/registrar`, no se inserta a mano.

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
- `GET /api/clientes?buscar=` - Listar clientes (filtra por apellidos, nombres o DNI)
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

### Proveedores (`/api/proveedores`)
- `GET /api/proveedores?buscar=` - Listar proveedores (filtra por razón social, RUC o correo)
- `GET /api/proveedores/{id}` - Buscar proveedor por ID
- `POST /api/proveedores` - Crear proveedor
- `PUT /api/proveedores/{id}` - Actualizar proveedor
- `DELETE /api/proveedores/{id}` - Eliminar proveedor

### Autenticación (`/api/auth`)
- `POST /api/auth/registrar` - Registrar usuario (rol `Usuario`)
- `POST /api/auth/login` - Iniciar sesión y obtener token JWT

---

## Documentación interactiva

Con la API en ejecución (perfil `Development`):

- Scalar: `https://localhost:7138/scalar`
- OpenAPI: `https://localhost:7138/openapi/v1.json`
