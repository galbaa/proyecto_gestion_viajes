using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class UsuarioRequest
    {
        public int UsuarioId { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public string Nombre { get; set; }
        public bool EsEmpleado { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }
    }
}