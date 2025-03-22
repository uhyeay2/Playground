using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.Database
{
    public class CreateTable_Profiles : DataExecuteWithoutParameters
    {
        public override string GetSql() =>
        $@"
        CREATE TABLE IF NOT EXISTS {Table.Profiles} (
            Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
            Name TEXT NOT NULL,
            CreatedDateTimeUTC TEXT NOT NULL,
            LastPlayedDateTimeUTC TEXT NOT NULL
        );";
    }
}
