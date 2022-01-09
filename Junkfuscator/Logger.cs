using Crayon;
using System;

namespace Junkfuscator
{
    class Logger
    {
        public static void Info(string x)
        {
            Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Blue("INFO")}{Output.Yellow("]")}{Output.Yellow("[")}{Output.Blue(DateTime.Now.ToString("hh:mm:ss").ToString())}{Output.Yellow("]")}: " + "{0}", x));
        }
        public static void Error(string x)
        {
            Console.WriteLine(String.Format($"{Output.Yellow("[")}{Output.Red("Error")}{Output.Yellow("]")}{Output.Yellow("[")}{Output.Blue(DateTime.Now.ToString("hh:mm:ss").ToString())}{Output.Yellow("]")}: " + "{0}", x));
        }
    }
}