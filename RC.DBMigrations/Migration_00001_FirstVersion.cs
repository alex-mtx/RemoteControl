using FluentMigrator;
using System;

namespace RC.DBMigrations
{
    [Migration(1)]
    public class Migration_00001_FirstVersion : Migration
    {
        public override void Down()
        {
            Delete.Table("command_request");
        }

        public override void Up()
        {
            Create.Table("command_request")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("RequestID").AsGuid().NotNullable()
                .WithColumn("SentOn").AsDateTime().NotNullable()
                .WithColumn("CmdType").AsInt32().NotNullable()
                .WithColumn("Finished").AsBoolean().WithDefault(0).NotNullable()
                      
                //Storage specific params
                .WithColumn("Path").AsString(1024).Nullable();

        }
    }
}
