using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models
{
    [Table("proveedores")]
    public class Proveedor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        //razón social
        [Required, StringLength(100)]
        [Column("razonsocial")]
        public string RazonSocial { get; set; } = string.Empty;

        //ruc
        [Required, StringLength(11)]
        [Column("ruc")]
        public string Ruc { get; set; } = string.Empty;

        //telefono
        [Required, StringLength(9)]
        [Column("telefono")]
        public string Telefono { get; set; } = string.Empty;

        //direccion
        [Required, StringLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; } = string.Empty;

        //correo
        [Required, StringLength(150)]
        [Column("correo")]
        public string Correo { get; set; } = string.Empty;
    }
}
