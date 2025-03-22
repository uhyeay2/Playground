using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.Enums;

namespace Playground.ConsoleUI.DisplayComponents
{
    public class DisplayedLine : DisplayComponent
    {
        #region Private Fields

        private readonly List<DisplayedText> _contents = [];

        private int _width;

        #endregion

        #region Constructors

        public DisplayedLine() { }

        public DisplayedLine(string text, TextColor color) => AddText(text, color);

        #endregion

        #region Public Accessors

        public IReadOnlyList<DisplayedText> Contents => _contents;

        #endregion

        #region Public Methods - Adding Text

        public DisplayedLine AddText(string text, TextColor color)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _contents.Add(new DisplayedText(text, color, new Position(DisplayedBounds.RightIndex + 1, DisplayedBounds.TopIndex)));
                _width += text.Length;
            }

            return this;
        }

        public DisplayedLine PrependText(string text, TextColor color)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _contents.Insert(0, new DisplayedText(text, color, new Position(DisplayedBounds.LeftIndex, DisplayedBounds.TopIndex)));
                _width += text.Length;

                foreach (var displayedText in _contents)
                {
                    displayedText.Position.X += text.Length;
                }
            }

            return this;
        }

        public DisplayedLine ApplyPadding(int targetWidth, ConsoleColor backgroundColor, Padding padding = Padding.Centered)
        {
            var (leftPadding, rightPadding) = padding.GetPaddingSize(_width, targetWidth);

            if (leftPadding > 0)
            {
                PrependText(new string(' ', leftPadding), new TextColor(backgroundColor));
            }

            if (rightPadding > 0)
            {
                AddText(new string(' ', rightPadding), new TextColor(backgroundColor));
            }

            return this;
        }

        #endregion

        #region IDisplayComponent Implementation

        public override IEnumerable<DisplayedLine> GetContents() => [this];

        public override int GetHeight() => 1;

        public override int GetWidth() => _width;

        public override void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;
            
            var width = 0;

            foreach (var displayedText in _contents)
            {
                displayedText.Position.X = x + width;

                displayedText.Position.Y = y;

                width += displayedText.Value.Length;
            }
        }

        #endregion
    }
}
