namespace QuotationAppv1.Migrations
{
    using QuotationAppv1.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<QuotationAppv1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QuotationAppv1.Models.ApplicationDbContext";
        }

        protected override void Seed(QuotationAppv1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            if (context.Categories.Count() == 0)
            {
                context.Categories.Add(new Category { Name = "Computers" });
                context.Categories.Add(new Category { Name = "History" });
                context.Categories.Add(new Category { Name = "Humor" });
                context.Categories.Add(new Category { Name = "Technology" });
                context.SaveChanges(); 

            }
            
        }
    }
}
