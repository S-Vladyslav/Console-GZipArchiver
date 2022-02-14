using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipArchiver
{
    static class ConsoleParser
    {
        public static string[] ConsoleArgumentsParsing(string[] args, out string outName, out string action)
        {
            if (args.Length == 0 || args[0].ToLower() == "-h" || args[0].ToLower() == "--help")
            {
                WriteHelpAndCloseApp();
            }

            if (args[0] == "--default" || args[0] == "-f" || args.Length > 3)
            {
                outName = "default";
            }
            else
            {
                outName = args[0];
            }

            if (args[1].ToLower() == "--compress" || args[1].ToLower() == "-c")
            {
                action = "c";
            }
            else if (args[1].ToLower() == "--decompress" || args[1].ToLower() == "-d")
            {
                action = "d";
            }
            else
            {
                foreach(var i in args)
                {
                    Console.WriteLine(i);
                }
                action = "";
                WriteHelpAndCloseApp();
            }

            List<string> inNames = new List<string>();

            for (int i = 2; i < args.Length; i++)
            {
                inNames.Add(args[i]);
            }

            return inNames.ToArray();
        }

        public static void WriteHelpAndCloseApp()
        {
            Console.WriteLine("*****  GZip Archiver  *****\nArguments: [Output File] [Action] [Input Files]\n");
            Console.WriteLine("[Output File]:\n\tEnter file name for output archive\n\t--default, -f - for default file name(*intup file name*.zip)\n\t*If input files number more than 1, output archive names set as default\n");
            Console.WriteLine("[Action]:\n\t--compress, -c - for compress input file\n\t--decompress, -d - for decompress input file\n");
            Console.WriteLine("[Input Files]:\n\tEnter space-separated names of files that will be archived\n");
            Console.WriteLine("Help:\n\t--help, -h");
            Environment.Exit(0);
        }
    }
}
