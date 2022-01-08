using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Junkfuscator.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator.Core.Protections
{
    public static class ROTStrings
    {
        private static int protectedStringsNumber = 0;
        internal static void Execute(ModuleDef mod)
        {
            try
            {
                ModuleDefMD typeModule = ModuleDefMD.Load(typeof(ROTStrings).Module);
                TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.ROTedStrings).MetadataToken));
                IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, mod.GlobalType, mod);
                MethodDef init = (MethodDef)members.Single(method => method.Name == "ROTMe");

                foreach (IDnlibDef member in members)
                    member.Name = "SGlkZVRoaXNGdW5ueVNT";

                foreach (TypeDef type in mod.Types)
                {
                    //if (type.IsGlobalModuleType) continue;
                    foreach (MethodDef method in type.Methods.Where(x => x.HasBody))
                    {
                        for (int i = 0; i < method.Body.Instructions.Count; i++)
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr && !method.Body.Instructions[i].Operand.ToString().Contains(Convert.ToBase64String(Encoding.UTF8.GetBytes("ց֥֥֞֨ՙ֐֥֨֫֝ՙ՚"))) && !Settings.no.Contains(method.Body.Instructions[i].Operand) && !type.Name.Contains("AssemblyLoader")) //Assembly Loader for Costura
                            {
                                if (method.Body.Instructions[i].Operand == Settings.WMB64) continue;
                                int rdmed = Generator.rdm.Next(1337, 7331);
                                var shiftedString = Helpers.ROTStrings.ROTtMe(method.Body.Instructions[i].Operand.ToString(), short.Parse(rdmed.ToString()));

                                method.Body.Instructions[i].OpCode = OpCodes.Ldstr;
                                //method.Body.Instructions[i].Operand = shiftedString;
                                method.Body.Instructions[i].Operand = Convert.ToBase64String(Encoding.UTF8.GetBytes(shiftedString));

                                method.Body.Instructions.Insert(i + 1, OpCodes.Ldc_I4.ToInstruction(-rdmed));
                                method.Body.Instructions.Insert(i + 2, OpCodes.Ldstr.ToInstruction(Settings.WMB64));
                                method.Body.Instructions.Insert(i + 3, OpCodes.Call.ToInstruction(init));

                                method.Body.SimplifyBranches();
                                method.Body.OptimizeBranches();

                                protectedStringsNumber++;
                                i += 2;
                            }
                    }
                }
                Logger.Info(String.Format("Shifted {0} strings.", Output.Green(protectedStringsNumber.ToString())));
                protectedStringsNumber = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}
