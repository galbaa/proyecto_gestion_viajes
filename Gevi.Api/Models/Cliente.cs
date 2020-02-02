using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gevi.Api.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Nombre { get; set; }
        public List<Proyecto> Proyectos { get; set; }
        public TipoCliente Tipo { get; set; }
    }
}