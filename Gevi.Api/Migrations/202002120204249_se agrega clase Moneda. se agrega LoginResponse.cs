namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seagregaclaseMonedaseagregaLoginResponse : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Monedas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Simbolo = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Gastoes", "Moneda_Id", c => c.Int());
            CreateIndex("dbo.Gastoes", "Moneda_Id");
            AddForeignKey("dbo.Gastoes", "Moneda_Id", "dbo.Monedas", "Id");
            DropColumn("dbo.Gastoes", "Moneda");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gastoes", "Moneda", c => c.String());
            DropForeignKey("dbo.Gastoes", "Moneda_Id", "dbo.Monedas");
            DropIndex("dbo.Gastoes", new[] { "Moneda_Id" });
            DropColumn("dbo.Gastoes", "Moneda_Id");
            DropTable("dbo.Monedas");
        }
    }
}
