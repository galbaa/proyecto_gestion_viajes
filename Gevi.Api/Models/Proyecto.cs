using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class Proyecto
    {
        public int Id { get; set; }
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