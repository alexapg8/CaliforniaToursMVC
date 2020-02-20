namespace CaliforniaTours.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bridge : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TourBookings", "GuestID", "dbo.Guests");
            DropForeignKey("dbo.Guests", "TourBooking_BookingID", "dbo.TourBookings");
            DropForeignKey("dbo.TourBookings", "Guest_GuestID", "dbo.Guests");
            DropIndex("dbo.Guests", new[] { "TourBooking_BookingID" });
            DropIndex("dbo.TourBookings", new[] { "GuestID" });
            DropIndex("dbo.TourBookings", new[] { "Guest_GuestID" });
            CreateTable(
                "dbo.TourBookingGuests",
                c => new
                    {
                        TourBooking_BookingID = c.Int(nullable: false),
                        Guest_GuestID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TourBooking_BookingID, t.Guest_GuestID })
                .ForeignKey("dbo.TourBookings", t => t.TourBooking_BookingID, cascadeDelete: true)
                .ForeignKey("dbo.Guests", t => t.Guest_GuestID, cascadeDelete: true)
                .Index(t => t.TourBooking_BookingID)
                .Index(t => t.Guest_GuestID);
            
            DropColumn("dbo.Guests", "TourBooking_BookingID");
            DropColumn("dbo.TourBookings", "Guest_GuestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TourBookings", "Guest_GuestID", c => c.Int());
            AddColumn("dbo.Guests", "TourBooking_BookingID", c => c.Int());
            DropForeignKey("dbo.TourBookingGuests", "Guest_GuestID", "dbo.Guests");
            DropForeignKey("dbo.TourBookingGuests", "TourBooking_BookingID", "dbo.TourBookings");
            DropIndex("dbo.TourBookingGuests", new[] { "Guest_GuestID" });
            DropIndex("dbo.TourBookingGuests", new[] { "TourBooking_BookingID" });
            DropTable("dbo.TourBookingGuests");
            CreateIndex("dbo.TourBookings", "Guest_GuestID");
            CreateIndex("dbo.TourBookings", "GuestID");
            CreateIndex("dbo.Guests", "TourBooking_BookingID");
            AddForeignKey("dbo.TourBookings", "Guest_GuestID", "dbo.Guests", "GuestID");
            AddForeignKey("dbo.Guests", "TourBooking_BookingID", "dbo.TourBookings", "BookingID");
            AddForeignKey("dbo.TourBookings", "GuestID", "dbo.Guests", "GuestID", cascadeDelete: true);
        }
    }
}
