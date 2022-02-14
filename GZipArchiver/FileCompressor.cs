using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace GZipArchiver
{
    class FileCompressor
    {
        public readonly string InputFileName;

        private readonly string Action;
        private readonly string OutputFileName;

        private Queue<byte[]> _fileReadingQueue = new Queue<byte[]>();
        private Queue<byte[]> _fileWritingQueue = new Queue<byte[]>();

        private const int _CHUNKSIZE = 10000;

        private bool _error = false;
        private bool _finalized = false;

        public FileCompressor(string outputFileName, string action, string inputFileName)
        {
            OutputFileName = outputFileName;
            Action = action;
            InputFileName = inputFileName;
        }

        public int StartFileCompression()
        {
            Console.WriteLine($"Start {InputFileName}");

            if (Action == "c")
            {
                Console.WriteLine($"Compressing {InputFileName}");
                FileReadForCompression();

                CompressChunks();

                WriteCompressedChunksToFile();

                _finalized = true;
            }
            else if (Action == "d")
            {
                Console.WriteLine($"Decompressing {InputFileName}");
                Decompress();
                _finalized = true;
            }

            if (_finalized && !_error)
            {
                Console.WriteLine($"File {InputFileName} was successful compressed");
                return 0;
            }
            return 1;
        }

        private void FileReadForCompression()
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileReadForCompression = new FileStream(InputFileName, FileMode.Open))
                {
                    int bytesForRead = _CHUNKSIZE;

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

        private void WriteCompressedChunksToFile()
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileCompressedWrite = new FileStream(OutputFileName, FileMode.Create))
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

        private void Decompress()
        {
            if (_error == true) return;

            try
            {
                using (FileStream fileReaderStream = new FileStream(InputFileName, FileMode.Open))
                {
                    using (FileStream fileWriterStream = File.Create(OutputFileName))
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
