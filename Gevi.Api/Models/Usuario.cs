using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gevi.Api.Models
{
    public abstract class Usuario
    {
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR")]
        [Index(IsUnique = true)]
        public string Email { get; set; }
        [Required]
        public string  Contrasenia { get; set; }
        [Required]
        public string Nombre { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro{ get; set; }

        public Usuario()
        {

        }

        public Usuario(string email, string contrasenia, string nombre, DateTime fechaRegistro)
        {
            this.Email = email;
            this.Contrasenia = contrasenia;
            this.Nombre = nombre;
            this.FechaRegistro = fechaRegistro;
        }
    }
}