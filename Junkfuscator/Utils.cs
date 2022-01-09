using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Junkfuscator
{
    class Utils
    {
        internal static void verifyFile(string x)
        {
            while (!File.Exists(x))
            {
                Console.Clear();
                Console.Write("File Path: ");
                verifyFile(Console.ReadLine().Replace("\"", string.Empty));
            }
            Program.filePath = x;
            
            string[] fileSplit = x.Split('\\');
            Program.fileName = fileSplit[fileSplit.Length - 1].ToString();

            Program.output = Program.filePath.Replace(Program.fileName, Program.fileName.Replace(".exe", "-addedLove.exe").Replace(".dll", "-addedLove.dll"));
            Console.Clear();
        }
    }
}