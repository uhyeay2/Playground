using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.Enums;
using Playground.ConsoleUtilities.Extensions;

namespace Playground.ConsoleUI.DisplayComponents
{
    public class DisplayedParagraph : DisplayComponent
    {
        #region Private Fields

        private readonly int _width;

        private readonly DisplayedBlock _displayedBlock = new();

        private DisplayedLine? _currentLine = null;

        #endregion

        #region Constructors

        public DisplayedParagraph(int width) =>  _width = width;

        public DisplayedParagraph(int width, string text, TextColor color, bool hyphenate = false, Padding padding = Padding.RightSide)
        : this(width) => Add(text, color, hyphenate, padding);

        #endregion

        #region Public Accessor

        public DisplayedBlock DisplayedBlock => _displayedBlock;

        #endregion

        #region Public Methods - Adding Content

        public DisplayedParagraph Add(string text, TextColor color, bool hyphenate = false, Padding padding = Padding.RightSide, int widthRequiredBeforeNewLineStarts = 3)
        {
            if (string.IsNullOrEmpty(text))
            {
                return this;
            }

            if (_currentLine?.GetWidth() >= _width)
            {
                _currentLine = null;
            }

            // If text fits into CurrentLine without exceeding the max width then add the text to currentLine.
            if (text.Length + _currentLine?.GetWidth() <= _width)
            {
                if (_currentLine == null)
                {
                    StartNewLine(new DisplayedLine(text, color));
                }
                else
                {
                    _currentLine.AddText(text, color);
                }

                return this;
            }

            // If currentLine is null then TextWrap the string and add each line
            if (_currentLine == null)
            {
                var lines = text.TextWrap(_width, hyphenate);

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines.ElementAt(i);

                    if (i == lines.Count - 1)
                    {
                        StartNewLine(new DisplayedLine(line, color));
                    }
                    else
                    {
                        StartNewLine(new DisplayedLine(line, color).ApplyPadding(_width, color.Background, padding));
                    }
                }

                return this;
            }

            // If currentLine was not null, need to finish the currentLine then we can TextWrap the rest.
            var widthRemainingForCurrentLine = _width - _currentLine.GetWidth();

            // If the remaining width is less than or equal to three characters, just add whitespace
            if (widthRemainingForCurrentLine <= widthRequiredBeforeNewLineStarts)
            {
                _currentLine.ApplyPadding(_width, color.Background, padding);

                // Use recursion to re-call this method and add the text.
                return Add(text, color, hyphenate, padding);
            }

            var indexOfLastSpace = text.LastIndexOf(' ', 0, widthRemainingForCurrentLine);

            string textToFinishRemainingLine;

            if (indexOfLastSpace > 0)
            {
                textToFinishRemainingLine = text.Substring(0, indexOfLastSpace);
                text = text.Remove(0, indexOfLastSpace);
            }
            else
            {
                if (hyphenate)
                {
                    textToFinishRemainingLine = text[..(widthRemainingForCurrentLine - 1)] + "-";
                    text = text.Remove(0, widthRemainingForCurrentLine - 1);
                }
                else
                {
                    textToFinishRemainingLine = text[..widthRemainingForCurrentLine];
                    text = text.Remove(0, widthRemainingForCurrentLine);
                }
            }

            _currentLine.AddText(textToFinishRemainingLine, color);

            // Use recursion to re-call this method and add the remaining text.
            return Add(text, color, hyphenate, padding);
        }

        public DisplayedParagraph EndLine(ConsoleColor backgroundColor, Padding padding = Padding.RightSide)
        {
            if (_currentLine != null && _currentLine.GetWidth() < _width)
            {
                _currentLine.ApplyPadding(_width, backgroundColor, padding);
            }

            _currentLine = null;

            return this;
        }

        public DisplayedParagraph AddLines(int count, ConsoleColor backgroundColor, Padding paddingToFinishCurrentLine = Padding.RightSide)
        {
            if (_currentLine != null && _currentLine.GetWidth() < _width)
            {
                EndLine(backgroundColor, paddingToFinishCurrentLine);
            }

            for (int i = 0; i < count; i++)
            {
                StartNewLine(new DisplayedLine(new string(' ', _width), new TextColor(backgroundColor)));
            }

            return this;
        }

        #endregion

        #region IDisplayComponent Implementation

        public override IEnumerable<DisplayedLine> GetContents() => _displayedBlock.GetContents();

        public override int GetHeight() => _displayedBlock.GetHeight();

        public override int GetWidth() => _width;

        public override void SetPosition(int x, int y)
        {
            _position.X = x;
            _position.Y = y;

            _displayedBlock.SetPosition(x, y);
        }

        #endregion

        #region Private Helper Methods

        private void StartNewLine(DisplayedLine line)
        {
            _currentLine = line;

            _displayedBlock.AddToBottom(line);
        }

        #endregion
    }
}
