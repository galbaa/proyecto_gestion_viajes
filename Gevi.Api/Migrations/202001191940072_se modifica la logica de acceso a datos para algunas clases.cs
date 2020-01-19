namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class semodificalalogicadeaccesoadatosparaalgunasclases : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TipoGastoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Gastoes", "Moneda", c => c.String());
            AddColumn("dbo.Gastoes", "Tipo_Id", c => c.Int());
            AlterColumn("dbo.Clientes", "Nombre", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            CreateIndex("dbo.Clientes", "Nombre", unique: true);
            CreateIndex("dbo.Gastoes", "Tipo_Id");
            AddForeignKey("dbo.Gastoes", "Tipo_Id", "dbo.TipoGastoes", "Id");
            DropColumn("dbo.Clientes", "Moneda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Moneda", c => c.String());
            DropForeignKey("dbo.Gastoes", "Tipo_Id", "dbo.TipoGastoes");
            DropIndex("dbo.Gastoes", new[] { "Tipo_Id" });
            DropIndex("dbo.Clientes", new[] { "Nombre" });
            AlterColumn("dbo.Clientes", "Nombre", c => c.String());
            DropColumn("dbo.Gastoes", "Tipo_Id");
            DropColumn("dbo.Gastoes", "Moneda");
            DropTable("dbo.TipoGastoes");
        }
    }
}
