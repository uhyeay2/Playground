using Playground.ConsoleUI.DisplayComponents;

namespace Playground.ConsoleUI.Core
{
    public interface IDisplayComponent
    {
        public int GetWidth();

        public int GetHeight();

        public Position GetPosition();

        public void SetPosition(int x, int y);

        public IEnumerable<DisplayedLine> GetContents();
    }
}
