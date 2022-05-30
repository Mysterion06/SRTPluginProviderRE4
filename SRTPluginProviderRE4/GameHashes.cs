using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderRE4
{
    /// <summary>
    /// SHA256 hashes for the RE5/BIO5 game executables.
    /// </summary>
    public static class GameHashes
    {
        private static readonly byte[] re4_1_1_0 = new byte[32] { 0x04, 0x2A, 0x3F, 0x46, 0x83, 0x71, 0xDB, 0x68, 0x67, 0x72, 0x82, 0xEE, 0xA7, 0x95, 0xBD, 0x07, 0x51, 0x06, 0x9E, 0x5B, 0x0D, 0x71, 0x14, 0x64, 0xC7, 0xEB, 0x5E, 0x54, 0x5A, 0x5F, 0x40, 0x60 };
        private static readonly byte[] re4_1_0_6 = new byte[32] { 0xB5, 0xFE, 0x55, 0x85, 0xE4, 0xD7, 0xD8, 0x76, 0x75, 0x15, 0x1A, 0xA5, 0xB9, 0x79, 0x6B, 0xE8, 0x87, 0xCA, 0x49, 0x0C, 0x84, 0x52, 0xE8, 0x3F, 0x23, 0x54, 0xB9, 0x97, 0x20, 0x70, 0x13, 0xCA };
        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using (SHA256 hashFunc = SHA256.Create())
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
                checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(re4_1_1_0))
            {
                Console.WriteLine("Steam Version Detected, 1.1.0");
                return GameVersion.RE4_1_1_0;
            } else if(checksum.SequenceEqual(re4_1_0_6)){
                Console.WriteLine("Steam Version Detected, 1.0.6");
                return GameVersion.RE4_1_0_6;
            }

            Console.WriteLine("Unknown Version");
            return GameVersion.Unknown;
        }
    }
}
