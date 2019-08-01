namespace iManual.Migrations
{
    using Models;
    using Models.Domains;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using iManual.Helper;
    using Models.EnumBase;

    internal sealed class Configuration : DbMigrationsConfiguration<iManual.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(iManual.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Roles.AddOrUpdate(
              p => p.Id,
              new ApplicationRole { Name = "Admin" },
              new ApplicationRole { Name = "Employee" }
            );
            //context.MainClaims.AddOrUpdate(
            //  p => p.Id,
            //  new MainClaim { Name = GlobalVariable.DefaultMainClaim , Active = true }
            //);
            //context.MainCategorys.AddOrUpdate(
            //    p => p.Id,
            //    new MainCategory { Name = "Main1",Active = true, CreatedBy = "UserId", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now }    
            //);
        }
    }
}
