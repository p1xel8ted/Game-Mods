using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace QModReloadedGUI
{
    internal static class Utilities
    {
        public static string CalculateMd5(string file)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(file);
            var hash = md5.ComputeHash(stream);
            var hashString = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            return hashString;
        }

        public static void WriteLog(string message, string gameLocation)
        {
            if (string.IsNullOrEmpty(gameLocation)) return;
            using var streamWriter = new StreamWriter(Path.Combine(gameLocation, "qmod_reloaded_log.txt"),
                true);
            streamWriter.WriteLine(message);
        }

        internal static string UpdateRequestCounts(NameValueCollection responseHeaders, string userName, bool premium)
        {
            if (responseHeaders == null) return string.Empty;
            var dailyRemaining = responseHeaders.GetValues("x-rl-daily-remaining")?[0];
            var dailyLimit = responseHeaders.GetValues("x-rl-daily-limit")?[0];
            return premium ? $@"{userName} (Premium), Daily Requests: {dailyRemaining}/{dailyLimit}" : $@"{userName} (Standard), Daily Requests: {dailyRemaining}/{dailyLimit}";
        }

        public class PairedKeys
        {
            public byte[] Lock { get; set; }
            public byte[] Vector { get; set; }
        }

        public class Validate
        {
            [JsonPropertyName("is_premium")]
            public bool IsPremium { get; set; }

            [JsonPropertyName("message")]
            public string Message { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("profile_url")]
            public string ProfileUrl { get; set; }
        }
        //thanks to Stardrop author for this
        internal class Obscure
        {
            public Obscure()
            {
                using var aes = Aes.Create();
                Key = aes.Key;
                Vector = aes.IV;
            }

            internal byte[] Key { get;}
            internal byte[] Vector { get;}
            internal static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
            {
                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

                using var msDecrypt = new MemoryStream(cipherText);
                using var csDecrypt = new CryptoStream(msDecrypt, decrypt, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                var plaintext = srDecrypt.ReadToEnd();

                return plaintext;
            }

            internal static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
            {
                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);

                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encrypt, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                var encrypted = msEncrypt.ToArray();

                return encrypted;
            }
        }
    }
}