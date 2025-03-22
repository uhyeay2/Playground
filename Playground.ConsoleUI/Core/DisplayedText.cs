namespace Playground.ConsoleUI.Core
{
    public class DisplayedText
    {
        public DisplayedText(string text, TextColor color, Position position)
        {
            Value = text;
            Color = color;
            Position = position;
        }

        public string Value { get; set; }

        public Position Position { get; set; }

        public TextColor Color { get; set; }
    }
}
