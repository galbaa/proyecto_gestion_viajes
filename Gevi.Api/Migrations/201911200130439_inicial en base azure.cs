namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicialenbaseazure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Contrasenia = c.String(nullable: false),
                        Nombre = c.String(nullable: false),
                        FechaRegistro = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Viajes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFin = c.DateTime(nullable: false),
                        Estado = c.String(),
                        Empleado_Id = c.Int(),
                        Proyecto_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Empleado_Id)
                .ForeignKey("dbo.Proyectoes", t => t.Proyecto_Id)
                .Index(t => t.Empleado_Id)
                .Index(t => t.Proyecto_Id);
            
            CreateTable(
                "dbo.Gastoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Estado = c.String(),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Fecha = c.DateTime(nullable: false),
                        Viaje_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Viajes", t => t.Viaje_Id)
                .Index(t => t.Viaje_Id);
            
            CreateTable(
                "dbo.Proyectoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Cliente_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Clientes", t => t.Cliente_Id)
                .Index(t => t.Cliente_Id);
            
            CreateTable(
                "dbo.Clientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Pais = c.String(),
                        Moneda = c.String(),
                        Tipo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoClientes", t => t.Tipo_Id)
                .Index(t => t.Tipo_Id);
            
            CreateTable(
                "dbo.TipoClientes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Viajes", "Proyecto_Id", "dbo.Proyectoes");
            DropForeignKey("dbo.Proyectoes", "Cliente_Id", "dbo.Clientes");
            DropForeignKey("dbo.Clientes", "Tipo_Id", "dbo.TipoClientes");
            DropForeignKey("dbo.Gastoes", "Viaje_Id", "dbo.Viajes");
            DropForeignKey("dbo.Viajes", "Empleado_Id", "dbo.Usuarios");
            DropIndex("dbo.Clientes", new[] { "Tipo_Id" });
            DropIndex("dbo.Proyectoes", new[] { "Cliente_Id" });
            DropIndex("dbo.Gastoes", new[] { "Viaje_Id" });
            DropIndex("dbo.Viajes", new[] { "Proyecto_Id" });
            DropIndex("dbo.Viajes", new[] { "Empleado_Id" });
            DropTable("dbo.TipoClientes");
            DropTable("dbo.Clientes");
            DropTable("dbo.Proyectoes");
            DropTable("dbo.Gastoes");
            DropTable("dbo.Viajes");
            DropTable("dbo.Usuarios");
        }
    }
}
