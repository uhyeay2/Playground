using Playground.ConsoleUI.Core;

namespace Playground.ConsoleUI.DisplayComponents
{
    public class DisplayedFill : DisplayComponent
    {
        private readonly int _width;

        private readonly int _height;

        private readonly TextColor _backgroundColor;

        public DisplayedFill(int width, int height, ConsoleColor backgroundColor)
        {
            _width = width;
            _height = height;
            _backgroundColor = new TextColor(backgroundColor);
        }

        public override IEnumerable<DisplayedLine> GetContents()
        {
            var contents = new DisplayedLine[_height];

            for (int i = 0; i < _height; i++)
            {
                var line = new DisplayedLine();

                line.AddText(new string(' ', _width), _backgroundColor);

                line.SetPosition(_position.X, _position.Y + i);

                contents[i] = line;
            }

            return contents;
        }

        public override int GetHeight() => _height;

        public override int GetWidth() => _width;

        public override void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;
        }
    }
}
