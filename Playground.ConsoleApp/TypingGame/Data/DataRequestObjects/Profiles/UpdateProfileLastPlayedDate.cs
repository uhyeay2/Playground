using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.DataRequestObjects.Profiles
{
    public class UpdateProfileLastPlayedDate : DataExecuteWithPropertyParameters
    {
        public UpdateProfileLastPlayedDate(int profileId)
        {
            ProfileId = profileId;
        }

        public int ProfileId { get; set; }

        public override string GetSql() =>
        $@"
            UPDATE {Table.Profiles}
            SET LastPlayedDateTimeUTC = DateTime('now', 'utc')
            WHERE Id = @ProfileId
        ;";
    }
}
