namespace Gevi.Api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seagregaempleadoalgasto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gastoes", "Empleado_Id", c => c.Int());
            CreateIndex("dbo.Gastoes", "Empleado_Id");
            AddForeignKey("dbo.Gastoes", "Empleado_Id", "dbo.Usuarios", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gastoes", "Empleado_Id", "dbo.Usuarios");
            DropIndex("dbo.Gastoes", new[] { "Empleado_Id" });
            DropColumn("dbo.Gastoes", "Empleado_Id");
        }
    }
}
