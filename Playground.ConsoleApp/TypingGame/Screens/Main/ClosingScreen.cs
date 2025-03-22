namespace Playground.ConsoleApp.TypingGame.Screens.Main
{
    internal class ClosingScreen : ConsoleScreen
    {
        public ClosingScreen(ConsoleApplication consoleApplication) : base(consoleApplication)
        {
        }

        protected override ConsoleScreen GetNextScreen() => this;
    }
}
