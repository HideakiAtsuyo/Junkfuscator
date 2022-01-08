using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Protections
{
    public static class Renamer
    {
        internal static void Execute(ModuleDef mod)
        {
            mod.EntryPoint.Name = "Junkfuscator";
            mod.EntryPoint.DeclaringType.Name = "Dot";
        }
    }
}
