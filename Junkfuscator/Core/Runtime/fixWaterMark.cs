using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Runtime
{
    public static class fixWaterMark
    {
        public static string fix(this string x)
        {
            return x.Replace(String.Format("|{0}", Convert.ToBase64String(Encoding.UTF8.GetBytes("github.com/HideakiAtsuyo"))), string.Empty);
        }
    }
}
