namespace Playground.ConsoleApp.TypingGame.Data.DataTransferObjects
{
    public class Profile_DTO
    {
        #region Public Properties / Table Columns

        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime CreatedDateTimeUTC { get; set; }

        public DateTime LastPlayedDateTimeUTC { get; set; }

        #endregion
    }
}
