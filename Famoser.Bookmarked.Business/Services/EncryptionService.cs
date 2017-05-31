using System;
using System.Text;
using Famoser.Bookmarked.Business.Services.Interfaces;
using PCLCrypto;

namespace Famoser.Bookmarked.Business.Services
{
    public class EncryptionService : IEncryptionService
    {
        /// <summary>
        /// random init Vector (I swear, I've rolled a dice!)
        /// </summary>
        private static readonly byte[] InitVector = {
                2, 1, 42, 14, 1, 2, 12, 4, 51, 21, 12, 3, 12, 3, 14, 12
            };

        public string Decrypt(string content, string password)
        {
            var key = Encoding.UTF8.GetBytes(password);
            return DecryptRaw(content, key);
        }

        public string Encrypt(string content, string password)
        {
            var key = Encoding.UTF8.GetBytes(password);
            return EncryptRaw(content, key);
        }

        public string EncryptRaw(string content, byte[] password)
        {
            var data = Encoding.UTF8.GetBytes(content);
            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symeticKey = provider.CreateSymmetricKey(password);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symeticKey, data, InitVector);
            return Convert.ToBase64String(bytes);
        }

        public string DecryptRaw(string content, byte[] password)
        {
            var data = Convert.FromBase64String(content);
            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            var symeticKey = provider.CreateSymmetricKey(password);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symeticKey, data, InitVector);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
