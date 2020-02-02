namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seagregafechainicioaproyectosequitapaisacliente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Proyectoes", "FechaInicio", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Gastoes", "Estado", c => c.Int(nullable: false));
            DropColumn("dbo.Clientes", "Pais");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clientes", "Pais", c => c.String());
            AlterColumn("dbo.Gastoes", "Estado", c => c.String());
            DropColumn("dbo.Proyectoes", "FechaInicio");
        }
    }
}
