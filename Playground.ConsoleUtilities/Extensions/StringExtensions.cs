namespace Playground.ConsoleUtilities.Extensions
{
    public static class StringExtensions
    {
        public static List<string> TextWrap(this string value, int width, bool hyphenate = false)
        {
            if (width <= 0)
            {
                throw new ArgumentOutOfRangeException("Width cannot be less than zero");
            }

            if (string.IsNullOrEmpty(value))
            {
                return [];
            }

            if (value.Length < width)
            {
                return [value];
            }

            var lines = new List<string>();

            var currentIndex = 0;

            while (currentIndex < value.Length)
            {
                string line;

                if (value.Length - currentIndex <= width)
                {
                    line = value.Substring(currentIndex, value.Length - currentIndex);
                }
                else
                {
                    var indexOfLastSpace = value.LastIndexOf(' ', currentIndex + width, width);

                    if (indexOfLastSpace > 0)
                    {
                        line = value.Substring(currentIndex, indexOfLastSpace - currentIndex);

                        // Increase currentIndex to account for the space turning into a new line.
                        currentIndex++;
                    }
                    else // No space found
                    {
                        if (hyphenate)
                        {
                            line = value.Substring(currentIndex, width - 1) + "-";

                            //Decrease currentIndex to account for adding a hyphen that isn't in the source text.
                            currentIndex--;
                        }
                        else
                        {
                            line = value.Substring(currentIndex, width);
                        }
                    }
                }

                currentIndex = currentIndex + line.Length;
                lines.Add(line);
            }

            return lines;
        }

        public static string PadToCenter(this string value, int targetLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string(' ', targetLength);
            }

            if (value.Length >= targetLength)
            {
                return value;
            }

            var leftPadding = (targetLength - value.Length) / 2;

            return value.PadLeft(value.Length + leftPadding).PadRight(targetLength);
        }
    }
}
