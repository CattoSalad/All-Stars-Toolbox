using System;
using System.IO;
using System.Linq;
using LZ4;

namespace astoolbox.modules
{
    public class NROLZHandler : IHandler
    {
        public void Extract(string sourcePath, string targetPath)
        {
            byte[] data = File.ReadAllBytes(sourcePath);
            byte[] sizeBytes = new byte[0x04];
            
            Array.Copy(data, sizeBytes, 0x04);
            Array.Reverse(sizeBytes);
            
            int beSize = BitConverter.ToInt32(sizeBytes);
            byte[] decompressed = LZ4Codec.Decode(data, 0x04, data.Length - 0x04, beSize);
            
            File.WriteAllBytes(targetPath, decompressed);
        }

        public void Compress(string sourcePath, string targetPath)
        {
            byte[] data = File.ReadAllBytes(sourcePath);
            byte[] sizeBytes = BitConverter.GetBytes(data.Length);
            
            Array.Reverse(sizeBytes);
            
            byte[] compressed = LZ4Codec.Encode(data, 0, data.Length);
            byte[] result = sizeBytes.Concat(compressed).ToArray();

            File.WriteAllBytes(targetPath, result);
        }
    }
}