using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;

namespace Junkfuscator.Core.Protections
{
    public static class Watermark
    {
        internal static void Execute(ModuleDefMD mod)
        {
            //https://github.com/yck1509/ConfuserEx/blob/master/Confuser.Core/ConfuserEngine.cs#L305-L326
            try
            {
                TypeRef attrRef = mod.CorLibTypes.GetTypeRef("System", "Attribute");
                var attrType = new TypeDefUser("", "GitHub", attrRef);
                mod.Types.Add(attrType);
                //mod.Mark(attrType, null);

                var ctor = new MethodDefUser(".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void, mod.CorLibTypes.String), MethodImplAttributes.Managed, MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);
                ctor.Body = new CilBody();
                ctor.Body.MaxStack = 1;
                ctor.Body.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
                ctor.Body.Instructions.Add(OpCodes.Call.ToInstruction(new MemberRefUser(mod, ".ctor", MethodSig.CreateInstance(mod.CorLibTypes.Void), attrRef)));
                ctor.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
                attrType.Methods.Add(ctor);
                //marker.Mark(ctor, null);

                var attr = new CustomAttribute(ctor);
                attr.ConstructorArguments.Add(new CAArgument(mod.CorLibTypes.String, "HideakiAtsuyo"));

                mod.CustomAttributes.Add(attr);

                Logger.Info(String.Format("[ConfuserEx] Injected the {0} with success !", Output.Green("Watermark")));
            } catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
