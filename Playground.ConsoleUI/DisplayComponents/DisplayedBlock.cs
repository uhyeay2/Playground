using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.Enums;

namespace Playground.ConsoleUI.DisplayComponents
{
    public class DisplayedBlock : DisplayComponent
    {
        #region Private Members
        
        private readonly List<DisplayedLine> _displayedLines = [];

        #endregion

        #region Constructors

        public DisplayedBlock() { }

        public DisplayedBlock(IDisplayComponent displayComponent) => AddToBottom(displayComponent);

        #endregion

        #region Public Methods - Adding Content

        public DisplayedBlock AddToBottom(IDisplayComponent displayComponent) => AddToBottom(displayComponent.GetContents().ToArray());

        public DisplayedBlock AddToBottom(params DisplayedLine[] displayedLines)
        {
            foreach (var line in displayedLines)
            {
                line.SetPosition(_position.X, _position.Y + GetHeight());

                _displayedLines.Add(line);
            }

            return this;
        }

        public DisplayedBlock AddToTop(IDisplayComponent displayComponent) => AddToTop(displayComponent.GetContents().ToArray());

        public DisplayedBlock AddToTop(IEnumerable<DisplayedLine> displayedLines)
        {
            _displayedLines.InsertRange(0, displayedLines);

            SetPosition(_position.X, _position.Y);

            return this;
        }

        #endregion

        #region Public Methods - Padding Content

        public DisplayedBlock PadWidth(int targetWidth, ConsoleColor backgroundColor, Padding padding = Padding.Centered)
        {
            foreach (var line in _displayedLines)
            {
                line.ApplyPadding(targetWidth, backgroundColor, padding);
            }

            return this;
        }

        public DisplayedBlock PadHeight(int targetHeight, ConsoleColor backgroundColor, Alignment alignment = Alignment.Center, int? width = null)
        {
            var (top, bottom) = alignment.GetAlignmentHeight(GetHeight(), targetHeight);

            if (width.GetValueOrDefault() <= 0)
            {
                width = GetWidth();
            }

            AddToTop(new DisplayedFill(width.GetValueOrDefault(), top, backgroundColor));

            AddToBottom(new DisplayedFill(width.GetValueOrDefault(), bottom, backgroundColor));

            return this;
        }

        public DisplayedBlock PadContents(int targetWidth, int targetHeight, ConsoleColor backgroundColor, Alignment alignment = Alignment.Center, Padding padding = Padding.Centered)
        {
            PadWidth(targetWidth, backgroundColor, padding);
            PadHeight(targetHeight, backgroundColor, alignment);

            return this;
        }

        #endregion  

        #region IDisplayComponent Implementation

        public override IEnumerable<DisplayedLine> GetContents() => _displayedLines;

        public override int GetHeight() => _displayedLines.Count;

        public override int GetWidth() => _displayedLines.Max(_ => _.GetWidth());

        public override void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;

            for (int i = 0; i < _displayedLines.Count; i++)
            {
                _displayedLines[i].SetPosition(x, y + i);
            }
        }

        #endregion
    }
}
