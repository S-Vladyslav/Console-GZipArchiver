using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipArchiver
{
    static class ConsoleParser
    {
        public static Compressor ConsoleArgumentsParsing(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() == "-h" || args[0].ToLower() == "--help")
            {
                WriteHelpAndCloseApp();
            }

            string outName;

            if (args[0] == "--default" || args[0] == "-f" || args.Length > 3)
            {
                outName = "default";
            }
            else
            {
                outName = args[0];
            }

            string action = "";

            if (args[1].ToLower() == "--compress" || args[2].ToLower() == "-c")
            {
                action = "c";
            }
            else if (args[1].ToLower() == "--decompress" || args[2].ToLower() == "-d")
            {
                action = "d";
            }
            else
            {
                WriteHelpAndCloseApp();
            }

            List<string> inNames = new List<string>();

            for (int i = 2; i < args.Length; i++)
            {
                inNames.Add(args[i]);
            }

            return new Compressor(outName, action, inNames.ToArray());
        }

        public static void WriteHelpAndCloseApp()
        {
            Console.WriteLine("*help*");
            Environment.Exit(0);
        }
    }
}
