using System;
using System.Collections.Generic;
using System.IO;

namespace astoolbox.modules
{
    public class HARHandler : IHandler
    {
        public void Extract(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            List<int> offsets = new List<int>();
            byte[] data = null;
            data = File.ReadAllBytes(sourcePath);
            int fileCount = BitConverter.ToInt32(data, 0x04);
            int payloadLength = BitConverter.ToInt32(data, 0x08);
            for (int pos = 0x0C; pos < data.Length; pos++)
            {
                if (data[pos] == 0x89 && data[pos + 0x01] == 0x50 &&
                    data[pos + 0x02] == 0x4E && data[pos + 0x03] == 0x47)
                {
                    offsets.Add(pos);
                }
            }

            for (int fileId = 0; fileId < fileCount; fileId++)
            {
                int offset = offsets[fileId];
                int length = (fileId < fileCount - 1 ? offsets[fileId + 1] : data.Length) - offset;
                byte[] fileData = new byte[length];
                Array.Copy(data, offset, fileData, 0, length);

                File.WriteAllBytes($"{targetPath}/{fileId}.png", fileData);
            }

            Console.WriteLine($"Extracted {fileCount} files successfully!");
        }

        public void Compress(string sourcePath, string targetPath)
        {
            FileAttributes attr = File.GetAttributes(sourcePath);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("sourcePath has to be a directory!");
            }

            string[] files = Directory.GetFiles(sourcePath);

            List<byte> data = new List<byte>();
            data.AddRange(new byte[]
            {
                0x48, 0x41, 0x52, 0x43,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            });
            
            for(int i = 0; i<files.Length;i++)
            {
                data.AddRange(File.ReadAllBytes(Path.Join(sourcePath, $"{i}.png")));
            }

            byte[] archiveData = data.ToArray();
            int totalSize = archiveData.Length - 0x0C;
            archiveData[0x04] = (byte) files.Length;
            archiveData[0x05] = (byte) (files.Length >> 0x08);
            archiveData[0x06] = (byte) (files.Length >> 0x10);
            archiveData[0x07] = (byte) (files.Length >> 0x18);
            archiveData[0x08] = (byte) totalSize;
            archiveData[0x09] = (byte) (totalSize >> 0x08);
            archiveData[0x0A] = (byte) (totalSize >> 0x10);
            archiveData[0x0B] = (byte) (totalSize >> 0x18);

            File.WriteAllBytes(targetPath, archiveData);
        }
    }
}