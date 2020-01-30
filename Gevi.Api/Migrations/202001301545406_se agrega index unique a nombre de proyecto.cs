namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seagregaindexuniqueanombredeproyecto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Proyectoes", "Nombre", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            CreateIndex("dbo.Proyectoes", "Nombre", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Proyectoes", new[] { "Nombre" });
            AlterColumn("dbo.Proyectoes", "Nombre", c => c.String());
        }
    }
}
