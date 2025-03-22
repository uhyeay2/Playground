using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.Enums;

namespace Playground.ConsoleUI.DisplayComponents
{
    public class DisplayedRow : DisplayComponent
    {
        private readonly List<DisplayedBlock> _contents = [];

        private int _width;

        private int _height;

        #region Public Methods - Adding Content

        public DisplayedRow AddToRight(IDisplayComponent displayComponent) => AddToRight(new DisplayedBlock(displayComponent));

        public DisplayedRow AddToRight(DisplayedBlock block)
        {
            block.SetPosition(DisplayedBounds.RightIndex + 1, _position.Y);

            UpdateWidthAndHeight(block);

            _contents.Add(block);

            return this;
        }

        public DisplayedRow AddToLeft(IDisplayComponent displayComponent) => AddToLeft(new DisplayedBlock(displayComponent));

        public DisplayedRow AddToLeft(DisplayedBlock block)
        {
            UpdateWidthAndHeight(block);
            
            _contents.Add(block);

            SetPosition(_position.X, _position.Y);

            return this;
        }

        #endregion

        #region Public Methods - Padding Contents

        public DisplayedRow PadHeight(int targetHeight, ConsoleColor backgroundColor, Alignment alignment = Alignment.Center)
        {
            foreach (var block in _contents)
            {
                block.PadHeight(targetHeight, backgroundColor, alignment);
            }

            return this;
        }

        public DisplayedRow PadWidth(int targetWidth, ConsoleColor backgroundColor, Padding padding = Padding.Centered)
        {
            var (left, right) = padding.GetPaddingSize(_width, targetWidth);

            if (left > 0)
            {
                AddToLeft(new DisplayedFill(left, _height, backgroundColor));
            }

            if (right > 0)
            {
                AddToRight(new DisplayedFill(right, _height, backgroundColor));
            }

            return this;
        }

        public DisplayedRow PadContents(int targetWidth, int targetHeight, ConsoleColor backgroundColor, Alignment alignment = Alignment.Center, Padding padding = Padding.Centered)
        {
            PadHeight(targetHeight, backgroundColor, alignment);
            PadWidth(targetWidth, backgroundColor, padding);

            return this;
        }

        #endregion

        #region IDisplayComponent Implementation

        public override IEnumerable<DisplayedLine> GetContents()
        {
            if (_contents.Count == 0)
            {
                return [];
            }
            
            var lines = new List<DisplayedLine>();

            lines.AddRange(_contents.SelectMany(_ => _.GetContents()));

            return lines;
        }

        public override int GetHeight() => _height;

        public override int GetWidth() => _width;

        public override void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;

            foreach (var block in _contents)
            {
                block.SetPosition(x, y);

                x = block.DisplayedBounds.RightIndex + 1;
            }
        }

        #endregion

        #region Private Helper Methods

        private void UpdateWidthAndHeight(DisplayedBlock block)
        {
            _width += block.GetWidth();

            var height = block.GetHeight();

            if (height > _height)
            {
                _height = height;
            }
        }

        #endregion
    }
}
