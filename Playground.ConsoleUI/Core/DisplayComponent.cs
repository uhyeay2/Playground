using Playground.ConsoleUI.DisplayComponents;

namespace Playground.ConsoleUI.Core
{
    public abstract class DisplayComponent : IDisplayComponent
    {
        protected readonly Position _position = new();

        private readonly DisplayedBounds _displayedBounds;

        protected DisplayComponent() => _displayedBounds = new DisplayedBounds(this);
        
        public DisplayedBounds DisplayedBounds => _displayedBounds;

        public Position GetPosition() => _position;

        public abstract IEnumerable<DisplayedLine> GetContents();

        public abstract int GetHeight();
        
        public abstract int GetWidth();
        
        public abstract void SetPosition(int x, int y);
    }
}
