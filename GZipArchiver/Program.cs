using System;
using System.Collections.Generic;
using System.Threading;


namespace GZipArchiver
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            string[] fileNames = ConsoleParser.ConsoleArgumentsParsing(args, out string finalFileName, out string action);

            List<FileCompressor> filesForCompressionList = new List<FileCompressor>();

            int result;

            if (finalFileName == "default")
            {
                foreach (var i in fileNames)
                {
                    if (action == "c")
                    {
                        filesForCompressionList.Add(new FileCompressor(i + ".zip", action, i));
                    }
                    else if (action == "d")
                    {
                        filesForCompressionList.Add(new FileCompressor(i.Substring(0, i.Length - 4), action, i));
                    }
                }
                foreach (var i in filesForCompressionList)
                {
                    new Thread(new ThreadStart( () =>
                        {
                            result = i.StartFileCompression();
                            Console.WriteLine($"{i.InputFileName} {result}");
                        }
                    )).Start();
                        
                }
            }
            else
            {
                filesForCompressionList.Add(new FileCompressor(finalFileName, action, fileNames[0]));
                result = filesForCompressionList[0].StartFileCompression();
                Console.WriteLine($"{filesForCompressionList[0].InputFileName} {result}");
            }

            return 0;
        }

        static void Console_CancelKeyPress()
        {
            Console.WriteLine("Exiting");
            Environment.Exit(-1);
        }
        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console_CancelKeyPress();
        }
    }
}
