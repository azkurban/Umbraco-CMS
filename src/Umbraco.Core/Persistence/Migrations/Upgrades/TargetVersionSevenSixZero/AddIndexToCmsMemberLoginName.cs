using System.Linq;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Core.Persistence.SqlSyntax;

namespace Umbraco.Core.Persistence.Migrations.Upgrades.TargetVersionSevenSixZero
{
    [Migration("7.6.0", 0, Constants.System.UmbracoMigrationName)]
    public class AddIndexToCmsMemberLoginName : MigrationBase
    {
        public AddIndexToCmsMemberLoginName(ISqlSyntaxProvider sqlSyntax, ILogger logger)
            : base(sqlSyntax, logger)
        { }

        public override void Up()
        {
            var dbIndexes = SqlSyntax.GetDefinedIndexes(Context.Database)
                .Select(x => new DbIndexDefinition()
                {
                    TableName = x.Item1,
                    IndexName = x.Item2,
                    ColumnName = x.Item3,
                    IsUnique = x.Item4
                }).ToArray();

            //make sure it doesn't already exist
            if (dbIndexes.Any(x => x.IndexName.InvariantEquals("IX_cmsMember_LoginName")) == false)
            {
                Create.Index("IX_cmsMember_LoginName").OnTable("cmsMember")
                    .OnColumn("LoginName")
                    .Ascending()
                    .WithOptions()
                    .NonClustered();
            }
        }

        public override void Down()
        {
            Delete.Index("IX_cmsMember_LoginName").OnTable("cmsMember");
        }
    }
}