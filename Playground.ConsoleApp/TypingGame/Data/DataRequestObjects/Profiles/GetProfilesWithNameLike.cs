using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.ConsoleApp.TypingGame.Data.DataTransferObjects;
using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.DataRequestObjects.Profiles
{
    public class GetProfilesWithNameLike : DataFetchListWithPropertyParameters<Profile_DTO>
    {
        public GetProfilesWithNameLike(string name) => Name = name;

        public string Name { get; set; } = string.Empty;

        public override string GetSql() =>
        $@"
            SELECT * FROM {Table.Profiles} 
            WHERE Name LIKE '%' || @Name || '%' 
            ORDER BY LastPlayedDateTimeUTC DESC
        ;";
    }
}
