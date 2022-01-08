using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Protections
{
    public static class NopAttack
    {
        internal static void Execute(ModuleDef mod)
        {
            try
            {
                foreach (TypeDef type in mod.GetTypes())
                {
                    foreach (MethodDef method in type.Methods.Where(x => x.HasBody && !type.Name.Contains("AssemblyLoader") && !x.IsStaticConstructor && !x.DeclaringType.IsGlobalModuleType)) //Who Care About Empty Method Even For A Junk Fuscator ("Obfuscation" Time Go Brrrrrrr)
                    {
                        for (int i = 0; i < Settings.nopPerMethod; i++)
                        {
                            method.Body.Instructions.Insert(0, new Instruction(OpCodes.Nop));
                        }
                    }
                }
                Logger.Info(String.Format("Nop attack finished with {0} Nop added per method in each type (including junk methods ;))!", Output.Green(Settings.nopPerMethod.ToString())));
            } catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
