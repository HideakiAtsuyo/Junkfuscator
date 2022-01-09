using dnlib.DotNet;

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
