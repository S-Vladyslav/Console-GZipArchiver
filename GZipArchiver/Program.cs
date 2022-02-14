using System;
using System.Collections.Generic;


namespace GZipArchiver
{
    class Program
    {
        static void Main(string[] args)
        {

            var a = new Compressor();
            // a.FileReadForCompression(@"D:\=Test\VLADIKMAN.png");

            //Archiver.Compress(@"D:\пасворди.txt", @"D:\=Test\VLADIKMAN.zip");

            a.FileReadForCompression(@"D:\VLADIKMAN.png");
            a.CompressChunks();
            a.WriteCompressedChunksToFile(@"D:\=Test\VLADIKMAN.zip");

        }

       
    }
}
