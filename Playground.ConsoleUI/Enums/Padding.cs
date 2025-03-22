namespace Playground.ConsoleUI.Enums
{
    public enum Padding
    {
        None = 0,
        LeftSide = 1,
        RightSide = 2,
        Centered = 3
    }

    public static class PaddingExtensions
    {
        public static (int leftPadding, int rightPadding) GetPaddingSize(this Padding padding, int currentWidth, int targetWidth)
        {
            if (padding == Padding.None || currentWidth >= targetWidth)
            {
                return (0, 0);
            }

            if (padding == Padding.LeftSide)
            {
                return (targetWidth - currentWidth, 0);
            }

            if (padding == Padding.RightSide)
            {
                return (0, targetWidth - currentWidth);
            }

            if (padding == Padding.Centered)
            {
                var left = (targetWidth - currentWidth) / 2;
                var right = targetWidth - currentWidth - left;

                return (left, right);
            }

            return (0, 0);
        }
    }
}
