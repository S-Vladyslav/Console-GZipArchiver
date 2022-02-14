using System;
using System.IO;
using System.IO.Compression;

namespace GZipArchiver
{
    static class Archiver
    {
        public static void Compress(string sourceFile, string compressedFile)
        {
            using (FileStream fileReaderStream = new FileStream(sourceFile, FileMode.Open))
            {
                using (FileStream fileWriterStream = File.Create(compressedFile))
                {
                    using (GZipStream fileCompressorStream = new GZipStream(fileWriterStream, CompressionMode.Compress))
                    {
                        fileReaderStream.CopyTo(fileCompressorStream);
                        Console.WriteLine($"{fileReaderStream.Length}\t{fileWriterStream.Length}");
                    }
                }
            }

            //return 0;
        }

        static int Decompress(string compressedFile, string targetFile)
        {
            using (FileStream fileReaderStream = new FileStream(compressedFile, FileMode.Open))
            {
                using (FileStream fileWriterStream = File.Create(targetFile))
                {
                    using (GZipStream fileDecompressorStream = new GZipStream(fileReaderStream, CompressionMode.Decompress))
                    {
                        fileDecompressorStream.CopyTo(fileWriterStream);
                        Console.WriteLine($"{fileReaderStream.Length}\t{fileWriterStream.Length}");
                    }
                }
            }

            return 0;
        }
    }
}
