using Playground.ConsoleUI;
using Playground.ConsoleUI.Core;
using Playground.ConsoleUI.DisplayComponents;
using Playground.ConsoleUI.Enums;

namespace Playground.ConsoleApp.TypingGame.Screens.Main
{
    public class OpeningScreen : ConsoleScreen
    {
        private readonly ConsoleColor _backgroundColor;
        private readonly TextColor _messageColor;
        private readonly DisplayedBlock _displayedBox;
        private readonly IDisplayComponent _eraser;
        private readonly IDisplayComponent _widthEraser;
        private readonly IDisplayComponent _heightEraser;

        private readonly int _boxWidth = 30;
        private readonly int _boxSidePadding = 4;
        private readonly int _boxHeightPadding = 2;

        private readonly int _millisecondsBetweenBoxMoving = 333;
        private double _millsecondsElapsedDuringLastRefresh;

        private int _xDirection = 1;
        private int _yDirection = 1;

        public OpeningScreen(ConsoleApplication consoleApplication) : base(consoleApplication)
        {
            _backgroundColor = ConsoleColor.Black;

            _messageColor = new TextColor(ConsoleColor.Black, ConsoleColor.Gray);

            var paragraph = new DisplayedParagraph(width: _boxWidth - 4)
                .Add("Welcome!", _messageColor)
                .AddLines(1, _messageColor.Background, paddingToFinishCurrentLine: Padding.Centered)
                .Add("I hope you enjoy the Typing Game! Press any key to continue.", _messageColor, padding: Padding.Centered)
                .AddLines(1, _messageColor.Background, paddingToFinishCurrentLine: Padding.Centered)
                .Add("Created by:", _messageColor).EndLine(_messageColor.Background, Padding.Centered)
                .Add("Daniel Aguirre", _messageColor).EndLine(_messageColor.Background, Padding.Centered)
                .DisplayedBlock;

            _displayedBox = paragraph.PadContents(
                targetWidth: _boxWidth + _boxSidePadding, 
                targetHeight: paragraph.GetHeight() + _boxHeightPadding, 
                backgroundColor: _messageColor.Background, 
                alignment: Alignment.Center, 
                padding: Padding.Centered
            );

            _displayedBox.SetPosition(
                x: (Console.WindowWidth - _displayedBox.GetWidth()) / 2, 
                y: (Console.WindowHeight - _displayedBox.GetHeight()) / 2
            );

            _eraser = new DisplayedFill(_displayedBox.GetWidth(), _displayedBox.GetHeight(), _backgroundColor);
            _widthEraser = new DisplayedFill(_displayedBox.GetWidth(), 1, _backgroundColor);
            _heightEraser = new DisplayedFill(1, _displayedBox.GetHeight(), _backgroundColor);
        }

        protected override ConsoleScreen GetNextScreen() => new ClosingScreen(_app);

        protected override bool IsFinished() => Console.KeyAvailable;

        protected override void Shutdown()
        {
            ClearScreen();

            Console.ReadKey(true);
        }

        protected override void Startup()
        {
            ClearScreen();

            Display(_displayedBox);

            _millsecondsElapsedDuringLastRefresh = _app.ElapsedTime.TotalMilliseconds;
        }

        protected override void Update()
        {       
            if (_app.ElapsedTime.TotalMilliseconds >= _millsecondsElapsedDuringLastRefresh + _millisecondsBetweenBoxMoving)
            {
                _millsecondsElapsedDuringLastRefresh = _app.ElapsedTime.TotalMilliseconds;

                SetDirections();

                UpdatePositions();

                Display(_heightEraser, _widthEraser, _displayedBox);
            }
        }

        private void SetDirections()
        {
            if (_displayedBox.DisplayedBounds.RightIndex >= Console.WindowWidth - 1)
            {
                _xDirection = -1;
            }
            else if (_displayedBox.DisplayedBounds.LeftIndex <= 0)
            {
                _xDirection = 1;
            }

            if (_displayedBox.DisplayedBounds.BottomIndex >= Console.WindowHeight - 1)
            {
                _yDirection = -1;
            }
            else if (_displayedBox.DisplayedBounds.TopIndex <= 0)
            {
                _yDirection = 1;
            }
        }

        private void UpdatePositions()
        {
            var currentPosition = _displayedBox.GetPosition();

            if (_xDirection > 0)
            {
                _heightEraser.SetPosition(
                    x: _displayedBox.DisplayedBounds.LeftIndex,
                    y: currentPosition.Y
                );
            }
            else
            {
                _heightEraser.SetPosition(
                   x: _displayedBox.DisplayedBounds.RightIndex,
                   y: currentPosition.Y
               );
            }

            if (_yDirection > 0)
            {
                _widthEraser.SetPosition(
                    x: currentPosition.X,
                    y: _displayedBox.DisplayedBounds.TopIndex
                );
            }
            else
            {
                _widthEraser.SetPosition(
                    x: currentPosition.X,
                    y: _displayedBox.DisplayedBounds.BottomIndex
                );
            }

            _displayedBox.SetPosition(currentPosition.X + _xDirection, currentPosition.Y + _yDirection);
        }

        private void ClearScreen() => Display(new DisplayedFill(Console.WindowWidth, Console.WindowHeight, _backgroundColor));
    }
}
