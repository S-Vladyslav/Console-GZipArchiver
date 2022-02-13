using System;
using System.Collections.Generic;


namespace GZipArchiver
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                WriteHelp();
                return;
            }

            Dictionary<string, string> argumentsDict = ParseArgs(args);


        }

        static Dictionary<string, string> ParseArgs(string[] args)
        {
            Dictionary<string, string> argsDict = new Dictionary<string, string>();

            argsDict.Add("archiveName", args[0]);
            argsDict.Add("action", args[1]);

            string files = "";

            for (int i = 2; i < args.Length; i++)
            {
                files += $" {args[i]}";
            }

            argsDict.Add("files", files);

            return new Dictionary<string, string>();
        }

        static void WriteHelp()
        {
            Console.WriteLine("*help*");
        }
    }
}
