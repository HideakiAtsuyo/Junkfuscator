using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Junkfuscator.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Junkfuscator.Core.Protections
{
    public static class StringEncoder
    {
        internal static void Execute(ModuleDef mod)
        {
            try
            {
                int strings = 0;
                ModuleDefMD typeModule = ModuleDefMD.Load(typeof(StringEncoder).Module);
                TypeDef typeDef = typeModule.ResolveTypeDef(MDToken.ToRID(typeof(Runtime.fixWaterMark).MetadataToken));
                IEnumerable<IDnlibDef> members = InjectHelper.Inject(typeDef, mod.GlobalType, mod);
                MethodDef init = (MethodDef)members.Single(method => method.Name == "fix");

                foreach (var type in mod.GetTypes())
                {
                    foreach (MethodDef method in type.Methods.Where(x => x.HasBody && !type.Name.Contains("AssemblyLoader") && !x.IsStaticConstructor && !x.DeclaringType.IsGlobalModuleType))
                    {
                        for (int i = 0; i < method.Body.Instructions.Count(); i++)
                        {
                            if (method.Body.Instructions[i].OpCode == OpCodes.Ldstr && !Settings.no.Contains(method.Body.Instructions[i].Operand))
                            {
                                strings++;
                                method.Body.Instructions[i].OpCode = OpCodes.Nop;
                                method.Body.Instructions.Insert(i + 1, new Instruction(OpCodes.Call, mod.Import(typeof(Encoding).GetMethod("get_UTF8", new Type[] { })))); // Load string onto stack
                                method.Body.Instructions.Insert(i + 2, new Instruction(OpCodes.Ldstr, String.Format("{0}|{1}", Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(method.Body.Instructions[i].Operand.ToString())), Settings.WMB64))); // Load string onto stack
                                method.Body.Instructions.Insert(i + 3, new Instruction(OpCodes.Call, init)); // Load string onto stack
                                method.Body.Instructions.Insert(i + 4, new Instruction(OpCodes.Call, mod.Import(typeof(Convert).GetMethod("FromBase64String", new Type[] { typeof(string) })))); // call method FromBase64String with string parameter loaded from stack, returned value will be loaded onto stack
                                method.Body.Instructions.Insert(i + 5, new Instruction(OpCodes.Callvirt, mod.Import(typeof(Encoding).GetMethod("GetString", new Type[] { typeof(byte[]) })))); // call method GetString with bytes parameter loaded from stack 
                                i += 5;
                            }
                        }
                    }
                }
                Logger.Info(String.Format("InvalidBase64 {0} strings !", Output.Green(strings.ToString())));
            } catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}