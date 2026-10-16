using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//Las anotaciones, definen los roles de los objetos

namespace ClientesApi.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required, StringLength(70)]
        [Column("apellidos")]
        public string Apellidos { get; set; } // apellidos NVARCHAR(70) NOT NULL

        //nombres
        [Required, StringLength(70)]
        [Column("nombres")]
        public string Nombres { get; set; }

        //telefono
        [Required, StringLength(9)]
        [Column("telefono")]
        public string Telefono { get; set; }

        //correo
        [Required, StringLength(150)]
        [Column("correo")]
        public string Correo { get; set; }

        //dni
        [Required, StringLength(8)]
        [Column("dni")]
        public string Dni { get; set; }

        //direccion
        [Required, StringLength(150)]
        [Column("direccion")]
        public string Direccion { get; set; }
    }
}
