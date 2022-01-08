using System;

namespace Junkfuscator.Core.Helpers
{
    public static class ROTStrings
    {
        public static string ROTtMe(this string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var ROTed = Convert.ToInt32(buffer[i]) + shift;

                if (ROTed > maxChar)
                {
                    ROTed -= maxChar;
                }
                else if (ROTed < Convert.ToInt32(char.MinValue))
                {
                    ROTed += maxChar;
                }
                buffer[i] = Convert.ToChar(ROTed);
            }

            return new string(buffer);
        }
    }
}
