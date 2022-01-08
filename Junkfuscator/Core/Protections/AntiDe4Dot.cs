using Crayon;
using dnlib.DotNet;
using System;

namespace Junkfuscator.Core.Protections
{
    public static class AntiDe4Dot
    {
        internal static void Execute(ModuleDef mod)
        {
            try
            {
                int interfaces = 0;
                InterfaceImpl interfaceM = new InterfaceImplUser(mod.GlobalType);
                TypeDef typeDef1 = new TypeDefUser("", Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(19, 20), 1), mod.CorLibTypes.GetTypeRef("System", "Attribute"));
                InterfaceImpl interface1 = new InterfaceImplUser(typeDef1);
                mod.Types.Add(typeDef1);
                typeDef1.Interfaces.Add(interface1);
                typeDef1.Interfaces.Add(interfaceM);
                interfaces += 2;
                for (int i = 0; i < Settings.AntiDe4DotInterfaces; i++)
                {
                    //string name = Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(10, 30), 1);
                    string name = Helpers.Generator.RandomString(Helpers.Generator.rdm.Next(50, 60), 1);
                    TypeDef typeDef2 = new TypeDefUser("", name, mod.CorLibTypes.GetTypeRef("System", "Attribute"));
                    InterfaceImpl interface2 = new InterfaceImplUser(typeDef2);
                    mod.Types.Add(typeDef2);
                    typeDef2.Interfaces.Add(interface2);
                    typeDef2.Interfaces.Add(interfaceM);
                    typeDef2.Interfaces.Add(interface1);

                    name = string.Empty;
                    interfaces += 6;
                }
                Logger.Info(String.Format("[Forgot] Injected {0} Anti-De4Dot Interfaces !", Output.Green(interfaces.ToString())));
            } catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}