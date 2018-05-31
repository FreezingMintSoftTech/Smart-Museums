using System.IO;
using System.IO.Compression;

using UnityEngine;

namespace Client
{
    class Compresser : MonoBehaviour
    {

        public static void DecompressZip(string zipPath, string destinationPath)
        {
            print("Zip Path: " + zipPath + ", Path: " + destinationPath + "2");
            if (!Directory.Exists(destinationPath))
            {
                print("Ok...");
                //Asta este pentru Visual Studio
                //ZipFile.ExtractToDirectory(zipPath, destinationPath);
                //Asta este pentru Unity
                ZipUtil.Unzip(zipPath, destinationPath);
            }
        }

        public static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }
    }
}
