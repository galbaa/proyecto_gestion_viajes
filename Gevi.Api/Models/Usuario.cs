using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public abstract class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string  Contrasenia { get; set; }
        [Required]
        public string Nombre { get; set; }
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