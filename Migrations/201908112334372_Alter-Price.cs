namespace ApiCatchFilms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Prices", "valid", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Prices", "valid");
        }
    }
}
