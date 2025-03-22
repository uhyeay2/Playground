﻿using Playground.ConsoleApp.TypingGame.Data.Database;
using Playground.ConsoleApp.TypingGame.Data.DataTransferObjects;
using Playground.Data.BaseRequests;

namespace Playground.ConsoleApp.TypingGame.Data.DataRequestObjects.Profiles
{
    public class GetLastProfilePlayed : DataFetchWithoutParameters<Profile_DTO>
    {
        public override string GetSql() =>
            $"SELECT * FROM {Table.Profiles} ORDER BY LastPlayedDateTimeUTC DESC LIMIT 1;";
    }

}
