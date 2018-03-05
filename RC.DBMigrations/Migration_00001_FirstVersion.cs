﻿using FluentMigrator;
using System;

namespace RC.DBMigrations
{
    [Migration(1)]
    public class Migration_00001_FirstVersion : Migration
    {
        public override void Down()
        {
            Delete.Table("[CmdParametersSets]");
        }

        public override void Up()
        {
            Create.Table("[CmdParametersSets]")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("RequestID").AsGuid().NotNullable()
                .WithColumn("SentOn").AsDateTime().NotNullable()
                .WithColumn("CmdType").AsInt32().NotNullable()
                .WithColumn("Status").AsInt32().NotNullable().WithDefaultValue(1) //awaiting for execution
                .WithColumn("Result").AsString(8 * 1024 * 1024).Nullable()
                .WithColumn("CmdResultJson").AsString(8 * 1024 * 1024).Nullable()
                .WithColumn("Issuer").AsString(1024).Nullable()

                //Storage specific params
                .WithColumn("Path").AsString(1024).Nullable();

        }
    }
}
