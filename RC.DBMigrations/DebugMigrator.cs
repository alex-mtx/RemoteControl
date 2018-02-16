using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SQLite;
using FluentMigrator.Runner.Processors.SqlServer;
using System;
using System.Reflection;

namespace RC.DBMigrations
{
    public class DebugMigrator
    {
        string connectionString;

        public DebugMigrator(string connectionString)
        {
            this.connectionString = connectionString;
        }

        private class MigrationOptions : IMigrationProcessorOptions
        {
            public bool PreviewOnly { get; set; }

            public string ProviderSwitches
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public int Timeout { get; set; }
        }
        /// <summary>
        /// Valid factory names:
        /// <para>SQLite</para>
        /// <para>SQLServer</para>
        /// </summary>
        /// <param name="processorFactory"></param>
        public void Migrate(string processorFactory = "SQLite")
        {
            var options = new MigrationOptions { PreviewOnly = false, Timeout = 0 };

            IMigrationProcessorFactory factory;

            if (processorFactory.Equals("SQLite", StringComparison.OrdinalIgnoreCase))
                factory = new SQLiteProcessorFactory();
            else if (processorFactory.Equals("SQLServer", StringComparison.OrdinalIgnoreCase))
                factory = new SqlServerProcessorFactory();
            else
                throw new ArgumentException("Expected SQLIte or SQLServer");

            var assembly = Assembly.GetExecutingAssembly();

            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var migrationContext = new RunnerContext(announcer)
            {
#if DEBUG
                // will create testdata
                Profile = "development"
#endif
            };

            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateDown(0,true);
                runner.MigrateUp(true);
            }
        }
    }
}
