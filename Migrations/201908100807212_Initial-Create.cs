namespace ApiCatchFilms.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        function_id = c.Int(nullable: false, identity: true),
                        movie_id = c.Int(nullable: false),
                        room_id = c.Int(nullable: false),
                        price_id = c.Int(nullable: false),
                        time = c.DateTime(nullable: false),
                        type = c.Int(nullable: false),
                        type_movie = c.Int(nullable: false),
                        description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.function_id)
                .ForeignKey("dbo.Movies", t => t.movie_id, cascadeDelete: true)
                .ForeignKey("dbo.Prices", t => t.price_id, cascadeDelete: true)
                .ForeignKey("dbo.Rooms", t => t.room_id, cascadeDelete: true)
                .Index(t => t.movie_id)
                .Index(t => t.room_id)
                .Index(t => t.price_id);
            
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        movie_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 30),
                        type = c.String(nullable: false, maxLength: 30),
                        description = c.String(nullable: false, maxLength: 500),
                        classification = c.String(nullable: false, maxLength: 30),
                        time = c.Time(nullable: false, precision: 7),
                        status = c.Int(nullable: false),
                        cover_url = c.String(maxLength: 800),
                        image_url = c.String(maxLength: 800),
                        rating = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.movie_id);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        price_id = c.Int(nullable: false, identity: true),
                        adult_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        child_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        old_man_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.price_id);
            
            CreateTable(
                "dbo.Rooms",
                c => new
                    {
                        room_id = c.Int(nullable: false, identity: true),
                        number = c.Int(nullable: false),
                        description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.room_id);
            
            CreateTable(
                "dbo.Room_seat",
                c => new
                    {
                        room_seat_id = c.Int(nullable: false, identity: true),
                        status = c.Int(nullable: false),
                        seat_id = c.Int(nullable: false),
                        room_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.room_seat_id)
                .ForeignKey("dbo.Rooms", t => t.room_id, cascadeDelete: true)
                .ForeignKey("dbo.Seats", t => t.seat_id, cascadeDelete: true)
                .Index(t => t.seat_id)
                .Index(t => t.room_id);
            
            CreateTable(
                "dbo.Seats",
                c => new
                    {
                        seat_id = c.Int(nullable: false, identity: true),
                        column = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.seat_id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        ticket_id = c.Int(nullable: false, identity: true),
                        create_at = c.DateTime(nullable: false),
                        room_seat_id = c.Int(nullable: false),
                        price_id = c.Int(nullable: false),
                        user_id = c.Int(nullable: false),
                        function_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ticket_id)
                .ForeignKey("dbo.Functions", t => t.function_id, cascadeDelete: true)
                .ForeignKey("dbo.Prices", t => t.price_id)
                .ForeignKey("dbo.Room_seat", t => t.room_seat_id)
                .ForeignKey("dbo.Users", t => t.user_id, cascadeDelete: true)
                .Index(t => t.room_seat_id)
                .Index(t => t.price_id)
                .Index(t => t.user_id)
                .Index(t => t.function_id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        first_name = c.String(nullable: false, maxLength: 30),
                        last_name = c.String(nullable: false, maxLength: 30),
                        email = c.String(nullable: false, maxLength: 40),
                        hire_date = c.DateTime(nullable: false),
                        user_name = c.String(nullable: false, maxLength: 30),
                        pass = c.String(nullable: false, maxLength: 30),
                        birth_date = c.DateTime(nullable: false),
                        rol = c.Int(nullable: false),
                        image_user_url = c.String(maxLength: 800),
                    })
                .PrimaryKey(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "user_id", "dbo.Users");
            DropForeignKey("dbo.Tickets", "room_seat_id", "dbo.Room_seat");
            DropForeignKey("dbo.Tickets", "price_id", "dbo.Prices");
            DropForeignKey("dbo.Tickets", "function_id", "dbo.Functions");
            DropForeignKey("dbo.Room_seat", "seat_id", "dbo.Seats");
            DropForeignKey("dbo.Room_seat", "room_id", "dbo.Rooms");
            DropForeignKey("dbo.Functions", "room_id", "dbo.Rooms");
            DropForeignKey("dbo.Functions", "price_id", "dbo.Prices");
            DropForeignKey("dbo.Functions", "movie_id", "dbo.Movies");
            DropIndex("dbo.Tickets", new[] { "function_id" });
            DropIndex("dbo.Tickets", new[] { "user_id" });
            DropIndex("dbo.Tickets", new[] { "price_id" });
            DropIndex("dbo.Tickets", new[] { "room_seat_id" });
            DropIndex("dbo.Room_seat", new[] { "room_id" });
            DropIndex("dbo.Room_seat", new[] { "seat_id" });
            DropIndex("dbo.Functions", new[] { "price_id" });
            DropIndex("dbo.Functions", new[] { "room_id" });
            DropIndex("dbo.Functions", new[] { "movie_id" });
            DropTable("dbo.Users");
            DropTable("dbo.Tickets");
            DropTable("dbo.Seats");
            DropTable("dbo.Room_seat");
            DropTable("dbo.Rooms");
            DropTable("dbo.Prices");
            DropTable("dbo.Movies");
            DropTable("dbo.Functions");
        }
    }
}
