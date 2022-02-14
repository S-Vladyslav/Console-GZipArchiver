using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZipArchiver
{
    class Compressor
    {
        Queue<byte[]> fileReadingQueue = new Queue<byte[]>();
        Queue<byte[]> fileWritingQueue = new Queue<byte[]>();
        int chunkSize = 10000;


        public void FileReadForCompression(string sourceFile)
        {
            using (FileStream fileReadForCompression = new FileStream(sourceFile, FileMode.Open))
            {
                int bytesForRead = chunkSize;

                while (fileReadForCompression.Position < fileReadForCompression.Length)
                {
                    if (fileReadForCompression.Position + bytesForRead > fileReadForCompression.Length)
                    {
                        bytesForRead = (int)(fileReadForCompression.Length - fileReadForCompression.Position);
                    }

                    byte[] chunkBuffer = new byte[bytesForRead];
                    fileReadForCompression.Read(chunkBuffer, 0, bytesForRead);
                    fileReadingQueue.Enqueue(chunkBuffer);
                }
                ;
            }
            ;
        }

        public void CompressChunks()
        {
            Console.WriteLine(fileReadingQueue.Count);
            while (true && fileReadingQueue.Count > 0)
            {
                Console.WriteLine(fileReadingQueue.Count);
                byte[] chunkForCompressing = fileReadingQueue.Dequeue();

                if (chunkForCompressing == null)
                {
                    return;
                }

                using (MemoryStream memory = new MemoryStream())
                {
                    using (GZipStream gzipCompressor = new GZipStream(memory, CompressionMode.Compress))
                    {
                        gzipCompressor.Write(chunkForCompressing, 0, chunkForCompressing.Length);
                    }

                    fileWritingQueue.Enqueue(memory.ToArray());
                }
            }
                

            #region easy
            //using (FileStream fileReaderStream = new FileStream(sourceFile, FileMode.Open))
            //{
            //    using (FileStream fileWriterStream = File.Create(compressedFile))
            //    {
            //        using (GZipStream fileCompressorStream = new GZipStream(fileWriterStream, CompressionMode.Compress))
            //        {
            //            fileReaderStream.CopyTo(fileCompressorStream);
            //            Console.WriteLine($"{fileReaderStream.Length}\t{fileWriterStream.Length}");
            //        }
            //    }
            //}

            //return 0;
            #endregion
        }

        public void WriteCompressedChunksToFile(string compressedFile)
        {
            using (FileStream fileCompressedWrite = new FileStream(compressedFile, FileMode.Create))
            {
                while(true && fileWritingQueue.Count > 0)
                {
                    byte[] compressedChunk = fileWritingQueue.Dequeue();

                    fileCompressedWrite.Write(compressedChunk);
                }
            }
        }
    }
}
