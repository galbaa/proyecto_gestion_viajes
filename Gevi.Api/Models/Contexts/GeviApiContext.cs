using System.Data.Entity;

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
    
        public GeviApiContext() : base("name=GeviDatabase")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Viaje> Viajes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<TipoCliente> TipoClientes { get; set; }
        public DbSet<TipoGasto> TipoGastos { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Moneda> Monedas{ get; set; }
    }
}
