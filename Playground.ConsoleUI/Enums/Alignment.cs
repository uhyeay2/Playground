namespace Playground.ConsoleUI.Enums
{
    public enum Alignment
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Center = 3
    }

    public static class AlignmentExtensions
    {
        public static (int top, int bottom) GetAlignmentHeight(this Alignment alignment, int currentHeight, int targetHeight)
        {
            if (alignment == Alignment.None || currentHeight >= targetHeight)
            {
                return (0, 0);
            }

            if (alignment == Alignment.Top)
            {
                return (targetHeight - currentHeight, 0);
            }

            if (alignment == Alignment.Bottom)
            {
                return (0, targetHeight - currentHeight);
            }

            if (alignment == Alignment.Center)
            {
                var top = (targetHeight - currentHeight) / 2;
                var bottom = targetHeight - currentHeight - top;

                return (top, bottom);
            }

            return (0, 0);
        }
    }
}
