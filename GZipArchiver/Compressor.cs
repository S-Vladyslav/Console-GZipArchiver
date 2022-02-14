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
        readonly string Action;
        readonly string[] InputFileNames;
        readonly string[] OutputFileName;

        private Queue<byte[]> _fileReadingQueue = new Queue<byte[]>();
        private Queue<byte[]> _fileWritingQueue = new Queue<byte[]>();
        private int _chunkSize = 10000;

        private bool _error = false;

        public Compressor(string outputFileName, string action, string[] inputFileNames)
        {
            if (outputFileName == "default")
            {
                OutputFileName = inputFileNames;
            }
            else
            {
                OutputFileName = new string[] { outputFileName };
            }
            Action = action;
            InputFileNames = inputFileNames;
        }

        private void FileReadForCompression(string sourceFile)
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileReadForCompression = new FileStream(sourceFile, FileMode.Open))
                {
                    int bytesForRead = _chunkSize;

                    while (fileReadForCompression.Position < fileReadForCompression.Length)
                    {
                        if (fileReadForCompression.Position + bytesForRead > fileReadForCompression.Length)
                        {
                            bytesForRead = (int)(fileReadForCompression.Length - fileReadForCompression.Position);
                        }

                        byte[] chunkBuffer = new byte[bytesForRead];
                        fileReadForCompression.Read(chunkBuffer, 0, bytesForRead);
                        _fileReadingQueue.Enqueue(chunkBuffer);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                _error = true;
            }
        }

        private void CompressChunks()
        {
            if (_error == true) return;

            try
            {
                while (true && _fileReadingQueue.Count > 0)
                {
                    byte[] chunkForCompressing = _fileReadingQueue.Dequeue();

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

                        _fileWritingQueue.Enqueue(memory.ToArray());
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                _error = true;
            }
        }

        private void WriteCompressedChunksToFile(string outputFileName)
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileCompressedWrite = new FileStream(outputFileName + ".zip", FileMode.Create))
                {
                    while (true && _fileWritingQueue.Count > 0)
                    {
                        byte[] compressedChunk = _fileWritingQueue.Dequeue();

                        fileCompressedWrite.Write(compressedChunk);
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                _error = true;
            }
        }

        private void Decompress(string compressedFile, string targetFile)
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileReaderStream = new FileStream(compressedFile, FileMode.Open))
                {
                    using (FileStream fileWriterStream = File.Create(targetFile))
                    {
                        using (GZipStream fileDecompressorStream = new GZipStream(fileReaderStream, CompressionMode.Decompress))
                        {
                            fileDecompressorStream.CopyTo(fileWriterStream);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                _error = true;
            }
        }
    }
}
