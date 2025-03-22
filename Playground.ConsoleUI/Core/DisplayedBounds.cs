namespace Playground.ConsoleUI.Core
{
    public class DisplayedBounds
    {
        private readonly IDisplayComponent _displayComponent;

        public DisplayedBounds(IDisplayComponent displayComponent) => _displayComponent = displayComponent;

        public int LeftIndex => _displayComponent.GetPosition().X;

        public int RightIndex => _displayComponent.GetPosition().X + _displayComponent.GetWidth() - 1;

        public int TopIndex => _displayComponent.GetPosition().Y;

        public int BottomIndex => _displayComponent.GetPosition().Y + _displayComponent.GetHeight() - 1;
    }
}
