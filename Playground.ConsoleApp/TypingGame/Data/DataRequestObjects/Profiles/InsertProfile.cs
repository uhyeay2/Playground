using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.DataRequestObjects.Profiles
{
    public class InsertProfile : DataExecuteWithPropertyParameters
    {
        public InsertProfile(string name) => Name = name;

        public string Name { get; set; }

        public override string GetSql() =>
        $@"
            INSERT INTO {Table.Profiles} (  Name,     CreatedDateTimeUTC, LastPlayedDateTimeUTC )
                                  VALUES ( @Name, DateTime('now', 'utc'), DateTime('now', 'utc') );
        ";
    }
}
