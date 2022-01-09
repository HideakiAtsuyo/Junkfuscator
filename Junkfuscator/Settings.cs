using System;
using System.Text;

namespace Junkfuscator
{
    public static class Settings
    {
        public static string WM = "github.com/HideakiAtsuyo", WMB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(WM));

        public static int AntiDe4DotInterfaces = 2000, Junks = 51000, junksMethods = 15, nopPerMethod = 3;
        public static string[] no = { "ZG5zcHk=", WM, WMB64 };
    }
}
