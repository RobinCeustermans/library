namespace Bibliotheek_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgeCategories",
                c => new
                    {
                        AgeCategoryID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.AgeCategoryID);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ISBN = c.String(),
                        SellPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReleaseDate = c.DateTime(nullable: false),
                        PurchaseDate = c.DateTime(nullable: false),
                        LifeSpan = c.DateTime(nullable: false),
                        AgeCategoryID = c.Int(nullable: false),
                        ItemTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemID)
                .ForeignKey("dbo.AgeCategories", t => t.AgeCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.ItemTypes", t => t.ItemTypeID, cascadeDelete: true)
                .Index(t => t.AgeCategoryID)
                .Index(t => t.ItemTypeID);
            
            CreateTable(
                "dbo.ContributorItems",
                c => new
                    {
                        ContributorItemID = c.Int(nullable: false, identity: true),
                        ContributorID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContributorItemID)
                .ForeignKey("dbo.Contributors", t => t.ContributorID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ContributorID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Contributors",
                c => new
                    {
                        ContributorID = c.Int(nullable: false, identity: true),
                        Surname = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        ContributorTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ContributorID)
                .ForeignKey("dbo.ContributorTypes", t => t.ContributorTypeID, cascadeDelete: true)
                .Index(t => t.ContributorTypeID);
            
            CreateTable(
                "dbo.ContributorTypes",
                c => new
                    {
                        ContributorTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ContributorTypeID);
            
            CreateTable(
                "dbo.Fines",
                c => new
                    {
                        FineID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        UserID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FineID)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Surname = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Country = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        StreetName = c.String(nullable: false),
                        HouseNumber = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        MembershipStartDate = c.DateTime(),
                        MembershipTypeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.MembershipTypes", t => t.MembershipTypeID, cascadeDelete: true)
                .Index(t => t.MembershipTypeID);
            
            CreateTable(
                "dbo.MembershipTypes",
                c => new
                    {
                        MembershipTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        MaxNumberItems = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MembershipTypeID);
            
            CreateTable(
                "dbo.UserBookFairs",
                c => new
                    {
                        UserBookFairID = c.Int(nullable: false, identity: true),
                        BookFairID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserBookFairID)
                .ForeignKey("dbo.BookFairs", t => t.BookFairID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.BookFairID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.BookFairs",
                c => new
                    {
                        BookFairID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Location = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.BookFairID);
            
            CreateTable(
                "dbo.UserInterestedItems",
                c => new
                    {
                        UserInterestedItemID = c.Int(nullable: false, identity: true),
                        DrawnDate = c.DateTime(),
                        ItemID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserInterestedItemID)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserItems",
                c => new
                    {
                        UserItemID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ReservedUntil = c.DateTime(),
                        BorrowedDate = c.DateTime(),
                        DueDate = c.DateTime(),
                        ReturnedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserItemID)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.ItemID);
            
            CreateTable(
                "dbo.ItemCategories",
                c => new
                    {
                        ItemCategoryID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ItemCategoryID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Items", t => t.ItemID, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.ItemTypes",
                c => new
                    {
                        ItemTypeID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ItemTypeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "ItemTypeID", "dbo.ItemTypes");
            DropForeignKey("dbo.ItemCategories", "ItemID", "dbo.Items");
            DropForeignKey("dbo.ItemCategories", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.UserItems", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.UserInterestedItems", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserInterestedItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.UserBookFairs", "UserID", "dbo.Users");
            DropForeignKey("dbo.UserBookFairs", "BookFairID", "dbo.BookFairs");
            DropForeignKey("dbo.Users", "MembershipTypeID", "dbo.MembershipTypes");
            DropForeignKey("dbo.Fines", "UserID", "dbo.Users");
            DropForeignKey("dbo.Fines", "ItemID", "dbo.Items");
            DropForeignKey("dbo.ContributorItems", "ItemID", "dbo.Items");
            DropForeignKey("dbo.Contributors", "ContributorTypeID", "dbo.ContributorTypes");
            DropForeignKey("dbo.ContributorItems", "ContributorID", "dbo.Contributors");
            DropForeignKey("dbo.Items", "AgeCategoryID", "dbo.AgeCategories");
            DropIndex("dbo.ItemCategories", new[] { "CategoryID" });
            DropIndex("dbo.ItemCategories", new[] { "ItemID" });
            DropIndex("dbo.UserItems", new[] { "ItemID" });
            DropIndex("dbo.UserItems", new[] { "UserID" });
            DropIndex("dbo.UserInterestedItems", new[] { "UserID" });
            DropIndex("dbo.UserInterestedItems", new[] { "ItemID" });
            DropIndex("dbo.UserBookFairs", new[] { "UserID" });
            DropIndex("dbo.UserBookFairs", new[] { "BookFairID" });
            DropIndex("dbo.Users", new[] { "MembershipTypeID" });
            DropIndex("dbo.Fines", new[] { "ItemID" });
            DropIndex("dbo.Fines", new[] { "UserID" });
            DropIndex("dbo.Contributors", new[] { "ContributorTypeID" });
            DropIndex("dbo.ContributorItems", new[] { "ItemID" });
            DropIndex("dbo.ContributorItems", new[] { "ContributorID" });
            DropIndex("dbo.Items", new[] { "ItemTypeID" });
            DropIndex("dbo.Items", new[] { "AgeCategoryID" });
            DropTable("dbo.ItemTypes");
            DropTable("dbo.Categories");
            DropTable("dbo.ItemCategories");
            DropTable("dbo.UserItems");
            DropTable("dbo.UserInterestedItems");
            DropTable("dbo.BookFairs");
            DropTable("dbo.UserBookFairs");
            DropTable("dbo.MembershipTypes");
            DropTable("dbo.Users");
            DropTable("dbo.Fines");
            DropTable("dbo.ContributorTypes");
            DropTable("dbo.Contributors");
            DropTable("dbo.ContributorItems");
            DropTable("dbo.Items");
            DropTable("dbo.AgeCategories");
        }
    }
}
