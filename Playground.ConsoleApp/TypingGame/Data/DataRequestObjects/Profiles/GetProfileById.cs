using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.ConsoleApp.TypingGame.Data.DataTransferObjects;
using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.DataRequestObjects.Profiles
{
    public class GetProfileById : DataFetchWithPropertyParameters<Profile_DTO>
    {
        public GetProfileById(int profileId) => ProfileId = profileId;

        public int ProfileId { get; set; }

        public override string GetSql() =>
            $"SELECT * FROM {Table.Profiles} WHERE Id = @ProfileId;";
    }

}
