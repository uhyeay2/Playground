namespace Playground.ConsoleUI.Core
{
    public class TextColor
    {
        /// <summary>
        /// Initialize an instance of TextColor where the fontColor and backgroundColor are two different colors.
        /// </summary>
        /// <param name="font">The ConsoleColor to use for the font when writing to the console.</param>
        /// <param name="background">The ConsoleColor to use for the background when writing to the console.</param>
        public TextColor(ConsoleColor font, ConsoleColor background)
        {
            Font = font;
            Background = background;
        }

        /// <summary>
        /// Initialize an instance of TextColor where the fontColor and backgroundColor are the same color.
        /// </summary>
        /// <param name="color">The ConsoleColor to use for the font and background when writing to the console.</param>
        public TextColor(ConsoleColor color) : this(color, color) { }

        /// <summary>
        /// The ConsoleColor to use for the font when writing to the console.
        /// </summary>
        public ConsoleColor Font { get; set; }

        /// <summary>
        /// The ConsoleColor to use for the background when writing to the console.
        /// </summary>
        public ConsoleColor Background { get; set; }
    }
}
