namespace Bibliotheek_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountActiveBookfairActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.BookFairs", "RegistrationOpen", c => c.Boolean(nullable: false, defaultValue: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookFairs", "RegistrationOpen");
            DropColumn("dbo.Users", "IsActive");
        }
    }
}
