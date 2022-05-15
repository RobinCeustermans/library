namespace Bibliotheek_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailTexts : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTexts");
        }
    }
}
