namespace TayanaYacht.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<TayanaYacht.Models.TayanaDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TayanaYacht.Models.TayanaDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
            if (!context.AdminUsers.Any())
            {
                context.AdminUsers.Add(new Models.AdminUser
                {
                    Username = "admin",
                    PasswordHash = System.Web.Helpers.Crypto.HashPassword("1234")
                });
            }
        }
    }
}
