using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipArchiver
{
    static class ConsoleParser
    {
        public static void ConsoleArgumentsParsing(string[] args)
        {
            if (args.Length == 0 || args[0].ToLower() == "-h" || args[0].ToLower() == "--help")
            {
                WriteHelp();
                return;
            }

            if (args[2].ToLower() == "compress")
            {
                ;
            }
            else if (args[2].ToLower() == "decompress")
            {
                ;
            }
            else
            {
                WriteHelp();
                return;
            }

            ;

            for (int i = 2; i < args.Length; i++)
            {
                ;
            }
        }

        public static void WriteHelp()
        {
            Console.WriteLine("*help*");
        }
    }
}
