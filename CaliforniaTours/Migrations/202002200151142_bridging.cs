namespace CaliforniaTours.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bridging : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TourBookings", "GuestID", "dbo.Guests");
            AddColumn("dbo.Guests", "TourBooking_BookingID", c => c.Int());
            AddColumn("dbo.TourBookings", "Guest_GuestID", c => c.Int());
            CreateIndex("dbo.Guests", "TourBooking_BookingID");
            CreateIndex("dbo.TourBookings", "Guest_GuestID");
            AddForeignKey("dbo.Guests", "TourBooking_BookingID", "dbo.TourBookings", "BookingID");
            AddForeignKey("dbo.TourBookings", "Guest_GuestID", "dbo.Guests", "GuestID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TourBookings", "Guest_GuestID", "dbo.Guests");
            DropForeignKey("dbo.Guests", "TourBooking_BookingID", "dbo.TourBookings");
            DropIndex("dbo.TourBookings", new[] { "Guest_GuestID" });
            DropIndex("dbo.Guests", new[] { "TourBooking_BookingID" });
            DropColumn("dbo.TourBookings", "Guest_GuestID");
            DropColumn("dbo.Guests", "TourBooking_BookingID");
            AddForeignKey("dbo.TourBookings", "GuestID", "dbo.Guests", "GuestID", cascadeDelete: true);
        }
    }
}
