namespace ApiCatchFilms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterSeat : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Seats", "row", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Seats", "row");
        }
    }
}
