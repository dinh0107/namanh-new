namespace hailinh.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<hailinh.DAL.DataEntities>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(hailinh.DAL.DataEntities context)
        {
            //  This method will be called after migrating to the latest version.
        }
    }
}
