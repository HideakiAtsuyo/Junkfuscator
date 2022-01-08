using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Junkfuscator.Core.Protections
{
    public static class ClassAndMethods
    {
        internal static void Execute(ModuleDef mod)
        {
            foreach (TypeDef type in mod.GetTypes())
            {
                for (int ii = 0; ii < Settings.junksMethods*4; ii++)
                {
                    var meth1 = new MethodDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 30), 1), MethodSig.CreateStatic(mod.CorLibTypes.Void), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                    type.Methods.Add(meth1);

                    meth1.Body = new CilBody()
                    {

                        Instructions =
                        {
                            Instruction.Create(OpCodes.Ldnull),
                        Instruction.Create(OpCodes.Throw)
                        }
                    };

                    //mod.GlobalType.Methods.Add(meth1);
                }
            }
            for (int i = 0; i < Settings.Junks; i++)
            {
                var junk2 = new TypeDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 30), 1), Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 30), 1), mod.CorLibTypes.Object.TypeDefOrRef);

                mod.Types.Add(junk2);

                for (int ii = 0; ii < Settings.junksMethods; ii++)
                {
                    var meth1 = new MethodDefUser(Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 30), 1), MethodSig.CreateStatic(mod.CorLibTypes.Object), MethodImplAttributes.IL | MethodImplAttributes.Managed, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.HideBySig | MethodAttributes.ReuseSlot);
                    junk2.Methods.Add(meth1);

                    meth1.Body = new CilBody()
                    {
                        Variables =
                    {
                        new Local(mod.CorLibTypes.Object)
                    },
                        Instructions =
                        {
                            Instruction.Create(OpCodes.Nop),
                        Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)13),
                        Instruction.Create(OpCodes.Newarr, mod.CorLibTypes.Object),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_0),
                        Instruction.Create(OpCodes.Ldstr, "H"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_1),
                        Instruction.Create(OpCodes.Ldstr, "i"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_2),
                        Instruction.Create(OpCodes.Ldstr, "d"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_3),
                        Instruction.Create(OpCodes.Ldstr, "e"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_4),
                        Instruction.Create(OpCodes.Ldstr, "a"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_5),
                        Instruction.Create(OpCodes.Ldstr, "k"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_6),
                        Instruction.Create(OpCodes.Ldstr, "i"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_7),
                        Instruction.Create(OpCodes.Ldstr, "A"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_8),
                        Instruction.Create(OpCodes.Ldstr, "t"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)9),
                        Instruction.Create(OpCodes.Ldstr, "s"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)10),
                        Instruction.Create(OpCodes.Ldstr, "u"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)11),
                        Instruction.Create(OpCodes.Ldstr, "y"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Dup),
                        Instruction.Create(OpCodes.Ldc_I4_S, (sbyte)12),
                        Instruction.Create(OpCodes.Ldstr, "o"),
                        Instruction.Create(OpCodes.Stelem_Ref),
                        Instruction.Create(OpCodes.Stloc_0),
                        Instruction.Create(OpCodes.Nop), // 56 => 57
                        Instruction.Create(OpCodes.Ldloc_0),
                        Instruction.Create(OpCodes.Ret)
                    }
                    };
                    meth1.Body.Instructions[56].OpCode = OpCodes.Br_S;
                    meth1.Body.Instructions[56].Operand = meth1.Body.Instructions[57];

                    //mod.GlobalType.Methods.Add(meth1);
                }
            }
        }
    }
}
