using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Linq;

namespace Junkfuscator.Core.Protections
{
    public static class HideMethods
    {
        internal static void Execute(ModuleDef mod)
        {
            try
            {
                int methodHidden = 0;
                foreach (TypeDef type in mod.Types.Where(x => x.HasMethods))
                {
                    foreach (MethodDef method in type.Methods.Where(x => x.HasBody))
                    {
                        method.Body.Instructions.Insert(0, new Instruction(OpCodes.Box, mod.Import(typeof(System.Math))));
                        methodHidden++;
                        /* Lol so little and easy to remove :( */
                    }
                }
                Logger.Info(String.Format("{0} Methods Hidden.", Output.Green(methodHidden.ToString())));
            } catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}