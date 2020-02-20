namespace CaliforniaTours.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.TourBookings", "GuestID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TourBookings", "GuestID", c => c.Int(nullable: false));
        }
    }
}
