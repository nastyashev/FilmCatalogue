namespace FilmCatalogue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 200),
                        parent_category_id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.parent_category_id)
                .Index(t => t.parent_category_id);
            
            CreateTable(
                "dbo.film_categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        film_id = c.Int(nullable: false),
                        category_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.category_id, cascadeDelete: true)
                .ForeignKey("dbo.films", t => t.film_id, cascadeDelete: true)
                .Index(t => t.film_id)
                .Index(t => t.category_id);
            
            CreateTable(
                "dbo.films",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 200),
                        director = c.String(nullable: false),
                        release = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "parent_category_id", "dbo.Categories");
            DropForeignKey("dbo.film_categories", "film_id", "dbo.films");
            DropForeignKey("dbo.film_categories", "category_id", "dbo.Categories");
            DropIndex("dbo.film_categories", new[] { "category_id" });
            DropIndex("dbo.film_categories", new[] { "film_id" });
            DropIndex("dbo.Categories", new[] { "parent_category_id" });
            DropTable("dbo.films");
            DropTable("dbo.film_categories");
            DropTable("dbo.Categories");
        }
    }
}
