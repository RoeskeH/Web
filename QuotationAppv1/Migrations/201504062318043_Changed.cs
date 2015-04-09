namespace QuotationAppv1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Quotations", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Quotations", "User_Id");
            AddForeignKey("dbo.Quotations", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Quotations", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Quotations", new[] { "User_Id" });
            DropColumn("dbo.Quotations", "User_Id");
        }
    }
}
