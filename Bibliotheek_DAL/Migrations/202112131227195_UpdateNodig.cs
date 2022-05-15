namespace Bibliotheek_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNodig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailTexts",
                c => new
                    {
                        EmailTextID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        HTMLString = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.EmailTextID);

            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false, defaultValue: true));
            AddColumn("dbo.BookFairs", "RegistrationOpen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookFairs", "RegistrationOpen");
            DropColumn("dbo.Users", "IsActive");
            DropTable("dbo.EmailTexts");
        }
    }
}
