using Crayon;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using Junkfuscator.Core.Protections;
using System;
using System.Linq;

namespace Junkfuscator
{
    class Program
    {
        public static string filePath = string.Empty, fileName = string.Empty, output = string.Empty;
        static void Main(string[] args)
        {
            Utils.verifyFile(args[0]);

            var Module = ModuleDefMD.Load(filePath);
            
            Logger.Info("Junkfuscating... I loved you I can't understand how you dare use it.. </3... ");

            Logger.Info("Encoding Strings...");
            StringEncoder.Execute(Module);

            Logger.Info("Applying Proxy Strings & Ints...");
            ProxyStringsAndInt.Execute(Module);

            //Logger.Info("Applying Proxy Methods...");
            //ProxyMethods.Execute(Module);

            Logger.Info("Injecting Anti-De4Dot Interfaces...");
            AntiDe4Dot.Execute(Module);

            Logger.Info("Shifting Strings...");
            ROTStrings.Execute(Module);

            if (!output.Contains("dll"))
            {
                Logger.Info("Renaming Entry Point & Entry Class...");
                Renamer.Execute(Module);
            }

            Logger.Info("[?] Applying Mutations...");
            Mutations.Booleanisator(Module);

            foreach (TypeDef typeDef in Module.Types.ToArray<TypeDef>())
            {
                foreach (MethodDef methodDef1 in typeDef.Methods.ToArray<MethodDef>().Where(x => x.HasBody && x.Body.Instructions.Count != 0))
                {
                    Mutations.Mutate1(methodDef1);
                }
            }

            Logger.Info("Applying CallToCalli...");
            CallToCalli.Execute(Module);

            Logger.Info("Injecting Junks [Class & Methods]...");
            ClassAndMethods.Execute(Module);

            Logger.Info(String.Format("NopAttack...({0} nop per method)", Output.Green(Settings.nopPerMethod.ToString())));
            NopAttack.Execute(Module);

            /*Logger.Info("Shifting Strings...");
            ROTStrings.Execute(Module);//A lot more lmao I hope you have some times to wait... => https://i.imgur.com/ou2FwWg.png
            */

            Logger.Info("[NetShield_Protector] Applying ControlFlow...");
            ControlFlow.Execute(Module);

            /*Logger.Info("Hiding methods..."); //Need to find a fix
            HideMethods.Execute(Module);*/

            Logger.Info("Injecting Watermark...");
            Watermark.Execute(Module);


            Logger.Info("Writing the file...");
            try
            {
                Module.Write(output);
                Logger.Info(String.Format("File {0} protected with success !", Output.Green(fileName)));
            }
            catch (Exception ex)
            {
                Logger.Info(String.Format("Failed to protect {0} !\nError: {1}", Output.Green(fileName), Output.Red(ex.Message)));

                ModuleWriterOptions x = new ModuleWriterOptions(Module);
                x.MetadataOptions.Flags = MetadataFlags.KeepOldMaxStack;
                Module.Write(output, x);
                Console.ReadLine();
                Environment.Exit(-1);
            }
            Console.ReadLine();
        }
    }
}