using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Gevi.Api.Models
{
    public class GeviApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public GeviApiContext() : base("name=GeviApiContext")
        {
        }

        public System.Data.Entity.DbSet<Gevi.Api.Models.Viaje> Viajes { get; set; }
    }
}
