using ClientesApi.Models; //Modelos de BD
using ProductosApi.Models; //Modelos de BD
using Microsoft.EntityFrameworkCore; //Acceder BD

namespace ClientesApi.Data;
//Herencia => subClase : superClase
public class AppDbContext : DbContext
{
    //Constructor (inicializar)
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    //ORM tendrá acceso al modelo cliente
    public DbSet<Cliente> Clientes { get; set; } //Clientes => tabla clientes
    public DbSet<Producto> Productos { get; set; } //Productos => tabla productos
    public DbSet<Usuario> Usuarios { get; set; } //Usuarios => tabla usuarios

}
