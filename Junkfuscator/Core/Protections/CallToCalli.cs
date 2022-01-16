using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.IO;
using System.Linq;

namespace Junkfuscator.Core.Protections
{
    public static class CallToCalli
    {
		//Before Constants and After Virt

		static string[] a = { "My.", ".My", "Costura" };
		static string[] b = { "Dispose", "ISupportInitialize", "Object" };
		private static MemberRef member;

		internal static void Execute(ModuleDef mod)
		{
			int CallToCalli = 0;
			MethodDef cctor = mod.GlobalType.FindOrCreateStaticConstructor();
			foreach (var type in mod.Types.ToArray())
			{
				foreach (var method in type.Methods.ToArray().Where(x => x.HasBody && !x.IsConstructor && !x.DeclaringType.IsGlobalModuleType && x.Body.HasInstructions && !a.Contains(x.FullName)))
				{

					for (int i = 0; i < method.Body.Instructions.Count; i++)
					{
						if (b.Contains(method.Body.Instructions[i].ToString()) || method.Body.Instructions[i].OpCode != OpCodes.Call && method.Body.Instructions[i].OpCode != OpCodes.Callvirt && method.Body.Instructions[i].OpCode != OpCodes.Ldloc_S) continue;
						bool flag3;
						if (method.Body.Instructions[i].OpCode == OpCodes.Call || method.Body.Instructions[i].OpCode == OpCodes.Callvirt)
						{
							object operand = method.Body.Instructions[i].Operand;
							member = (operand as MemberRef);
							flag3 = (member != null);
						}
						else
						{
							flag3 = false;
						}
						if (flag3)
						{
							//JustBeforeCalliShitAddNopToCounterSomeTools
							if (new FileInfo(mod.Location).Length > 1000000L)
							{
								method.Body.Instructions[i].OpCode = OpCodes.Ldftn;
								method.Body.Instructions[i].Operand = member;
								method.Body.Instructions.Insert(++i, Instruction.Create(OpCodes.Calli, member.MethodSig));
								CallToCalli++;
							}
							else
							{
								FieldDef field = new FieldDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(6, 20), 0), new FieldSig(mod.CorLibTypes.Object), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
								method.DeclaringType.Fields.Add(field);
								cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Stsfld, field));
								cctor.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Ldftn, member));
								method.Body.Instructions[i].OpCode = OpCodes.Ldsfld;
								method.Body.Instructions[i].Operand = field;
								method.Body.Instructions.Insert(++i, Instruction.Create(OpCodes.Calli, member.MethodSig));
								CallToCalli++;
							}
						}
					}
					method.Body.SimplifyBranches();
				}
			}
			Logger.Info(String.Format("[Forgot] {0} Call To Calli !", Output.Green(CallToCalli.ToString())));
		}
	}
}