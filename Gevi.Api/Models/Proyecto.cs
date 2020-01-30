using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gevi.Api.Models
{
    public class Proyecto
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }
        public Cliente Cliente { get; set; }

        public Proyecto()
        {

        }

        public Proyecto(string nombre, Cliente cliente)
        {
            this.Nombre = nombre;
            this.Cliente = cliente;
        }
    }
}