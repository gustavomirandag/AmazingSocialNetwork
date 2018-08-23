namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adicionadoemail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profiles", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profiles", "Email");
        }
    }
}
