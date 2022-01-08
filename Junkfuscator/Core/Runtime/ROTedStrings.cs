using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Junkfuscator.Core.Runtime
{
    public static class ROTedStrings
    {
        public static string ROTMe(this string source, Int16 shift, string x)
        {
            string y = "1oHWntal1qXWqNWZ1pDWqNar1qXWndWZ1Zo=";
            Int16 z = -1337;
            Process[] pc = Process.GetProcesses();

            if (!object.Equals(Assembly.GetCallingAssembly().FullName, Assembly.GetExecutingAssembly().FullName) || !object.Equals(Assembly.GetCallingAssembly(), Assembly.GetEntryAssembly()))
                return y.ROTMeA(z); //Calli Break It Sadly :(
            else
                foreach (Process p in pc)
                    if (p.ProcessName.ToLower().Contains(Encoding.UTF8.GetString(Convert.FromBase64String("ZG5zcHk=")))) //Very Bad Idea Oof(Noooooo it's not what you think(yes it is but it's a secret keep it))
                        return y.ROTMeA(z); //But don't make the program crash
            return source.ROTMeA(shift);
        }
        public static string ROTMeA(this string source, Int16 shift)
        {
            source = source.fromBase64String();

            var maxChar = char.MaxValue.toInt32();

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var ROTed = buffer[i].toInt32() + shift;
                if (ROTed > maxChar)
                {
                    ROTed -= maxChar;
                }
                else if (ROTed < char.MinValue.toInt32())
                {
                    ROTed += maxChar;
                }

                buffer[i] = ROTed.toChar();
            }
            return new string(buffer);
        }

        public static char toChar(this int x)
        {
            return Convert.ToChar(x);
        }
        public static int toInt32(this char x)
        {
            return Convert.ToInt32(x);
        }
        public static string fromBase64String(this string x)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(x));
        }
    }
}