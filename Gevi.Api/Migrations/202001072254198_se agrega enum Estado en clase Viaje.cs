namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seagregaenumEstadoenclaseViaje : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Viajes", "Estado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Viajes", "Estado", c => c.String());
        }
    }
}
