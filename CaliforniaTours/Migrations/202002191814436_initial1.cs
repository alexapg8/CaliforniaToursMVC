namespace CaliforniaTours.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Parks", "DepartureTime", c => c.String());
            AlterColumn("dbo.Parks", "ReturningTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Parks", "ReturningTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Parks", "DepartureTime", c => c.DateTime(nullable: false));
        }
    }
}
