namespace CaliforniaTours.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.GuestID);
            
            CreateTable(
                "dbo.TourBookings",
                c => new
                    {
                        BookingID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Cost = c.Int(nullable: false),
                        GuestID = c.Int(nullable: false),
                        ParkID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingID)
                .ForeignKey("dbo.Guests", t => t.GuestID, cascadeDelete: true)
                .ForeignKey("dbo.Parks", t => t.ParkID, cascadeDelete: true)
                .Index(t => t.GuestID)
                .Index(t => t.ParkID);
            
            CreateTable(
                "dbo.Parks",
                c => new
                    {
                        ParkID = c.Int(nullable: false, identity: true),
                        ParkName = c.String(),
                        DepartureTime = c.DateTime(nullable: false),
                        ReturningTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ParkID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TourBookings", "ParkID", "dbo.Parks");
            DropForeignKey("dbo.TourBookings", "GuestID", "dbo.Guests");
            DropIndex("dbo.TourBookings", new[] { "ParkID" });
            DropIndex("dbo.TourBookings", new[] { "GuestID" });
            DropTable("dbo.Parks");
            DropTable("dbo.TourBookings");
            DropTable("dbo.Guests");
        }
    }
}
