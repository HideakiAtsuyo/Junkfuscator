using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;

namespace Junkfuscator.Core.Protections
{
    public static class ProxyStringsAndInt
    {

        internal static void Execute(ModuleDef mod)
        {
            int proxiedStringsNumber = 0, proxiedIntNumber = 0;

            try
            {
                foreach (var type in mod.GetTypes())
                {
                    if (type.IsGlobalModuleType || type.Name.Contains("AssemblyLoader") || type.Name == "<Module>") continue;
                    foreach (var method in type.Methods)
                    {
                        if (!method.HasBody) continue;
                        var instr = method.Body.Instructions;
                        foreach (var t in instr)
                        {
                            if (t.OpCode == OpCodes.Ldstr)
                            {
                                //var meth1 = new MethodDefUser(RenamerHelper.RandomString(Others.rdm.Next(10, 20)), MethodSig.CreateStatic(mod.CorLibTypes.String), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                                var meth1 = new MethodDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 20), 1), MethodSig.CreateStatic(mod.CorLibTypes.String), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                                mod.GlobalType.Methods.Add(meth1);
                                meth1.Body = new CilBody();
                                meth1.Body.Variables.Add(new Local(mod.CorLibTypes.String, "text"));
                                meth1.Body.Variables.Add(new Local(mod.CorLibTypes.String));

                                meth1.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Nop));
                                meth1.Body.Instructions.Insert(1, Instruction.Create(OpCodes.Ldstr, t.Operand.ToString()));
                                meth1.Body.Instructions.Insert(2, Instruction.Create(OpCodes.Stloc_0));
                                meth1.Body.Instructions.Insert(3, Instruction.Create(OpCodes.Ldloc_0));
                                meth1.Body.Instructions.Insert(4, Instruction.Create(OpCodes.Dup));
                                //meth1.Body.Instructions.Insert(5, Instruction.Create(OpCodes.Brtrue_S, meth1.Body.Instructions[7]));
                                meth1.Body.Instructions.Insert(5, Instruction.Create(OpCodes.Nop)); //Replaced
                                meth1.Body.Instructions.Insert(6, Instruction.Create(OpCodes.Pop));
                                meth1.Body.Instructions.Insert(7, Instruction.Create(OpCodes.Ldstr, Settings.WMB64));
                                meth1.Body.Instructions.Insert(8, Instruction.Create(OpCodes.Stloc_1));
                                //meth1.Body.Instructions.Insert(9, Instruction.Create(OpCodes.Br_S, meth1.Body.Instructions[10]));
                                meth1.Body.Instructions.Insert(9, Instruction.Create(OpCodes.Nop)); //Replaced
                                meth1.Body.Instructions.Insert(10, Instruction.Create(OpCodes.Ldloc_1));
                                meth1.Body.Instructions.Insert(11, Instruction.Create(OpCodes.Ret));

                                //meth1.Body.Instructions.RemoveAt(5);
                                if (meth1.Body.Instructions[5].OpCode == OpCodes.Nop)
                                {
                                    meth1.Body.Instructions.RemoveAt(5);
                                    meth1.Body.Instructions[5].OpCode = OpCodes.Brtrue_S;
                                    meth1.Body.Instructions[5].Operand = meth1.Body.Instructions[7];
                                }
                                meth1.Body.Instructions.Insert(6, Instruction.Create(OpCodes.Pop));
                                meth1.Body.Instructions.Insert(10, Instruction.Create(OpCodes.Ldloc_1));
                                if (meth1.Body.Instructions[9].OpCode == OpCodes.Nop)
                                {
                                    meth1.Body.Instructions.RemoveAt(9);
                                    meth1.Body.Instructions[9].OpCode = OpCodes.Br_S;
                                    meth1.Body.Instructions[9].Operand = meth1.Body.Instructions[10];
                                }
                                meth1.Body.SimplifyBranches();
                                meth1.Body.OptimizeBranches();
                                t.OpCode = OpCodes.Call;
                                t.Operand = meth1;
                                proxiedStringsNumber++;
                            } else if (t.IsLdcI4())
                            {
                                var meth1 = new MethodDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 20), 1), MethodSig.CreateStatic(mod.CorLibTypes.Int32), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                                mod.GlobalType.Methods.Add(meth1);
                                meth1.Body = new CilBody();
                                meth1.Body.Variables.Add(new Local(mod.CorLibTypes.Int32));
                                meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_I4, t.GetLdcI4Value()));
                                meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                                meth1.Body.SimplifyBranches();
                                meth1.Body.OptimizeBranches();
                                t.OpCode = OpCodes.Call;
                                t.Operand = meth1;
                                proxiedIntNumber++;
                            }
                            else if (t.OpCode == OpCodes.Ldc_R4)
                            {
                                var meth1 = new MethodDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 20), 1), MethodSig.CreateStatic(mod.CorLibTypes.Double), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                                mod.GlobalType.Methods.Add(meth1);
                                meth1.Body = new CilBody();
                                meth1.Body.Variables.Add(new Local(mod.CorLibTypes.Double));
                                meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ldc_R4, (float)t.Operand));
                                meth1.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
                                meth1.Body.SimplifyBranches();
                                meth1.Body.OptimizeBranches();
                                t.OpCode = OpCodes.Call;
                                t.Operand = meth1;
                                proxiedIntNumber++;
                            }
                        }
                    }
                }
                Logger.Info(String.Format("Proxied {0} strings & {1} ints.", Output.Green(proxiedStringsNumber.ToString()), Output.Green(proxiedIntNumber.ToString())));
                proxiedStringsNumber = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}