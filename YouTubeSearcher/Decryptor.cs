using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YouTubeSearcher
{
    internal static class Decryptor
    {
        private const string PublicKey = "Mn_K6tCU";
        private const string IV = "Bp7kf7K*";

        private static byte[] PublicKeyBytes => Encoding.ASCII.GetBytes(PublicKey);
        private static byte[] IVBytes => Encoding.ASCII.GetBytes(IV);

        public static string Decrypt(string encrypted)
        {
            using var provider = new DESCryptoServiceProvider();
            var decryptor = provider.CreateDecryptor(PublicKeyBytes, IVBytes);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write);

            var buffer = Convert.FromBase64String(encrypted);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }
}