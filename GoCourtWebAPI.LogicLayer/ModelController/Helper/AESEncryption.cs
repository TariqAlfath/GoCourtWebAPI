using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Text;

namespace GoCourtWebAPI.LogicLayer.ModelController.Helper
{
    public class AESEncryption
    {
        private static readonly string key = "R#7sP9k@5t!3wY6X";
        private static readonly string initialitationValue = "Q3g@F9j3Rv$K1pZL";

        public static string Encrypt(string plainText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(initialitationValue);
            byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);

            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");
            KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("AES", keyBytes);
            ParametersWithIV ivParam = new ParametersWithIV(keyParam, ivBytes); // Menambahkan IV ke ParametersWithIV

            cipher.Init(true, ivParam);
            byte[] encryptedBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            int length = cipher.ProcessBytes(inputBytes, encryptedBytes, 0);
            cipher.DoFinal(encryptedBytes, length);

            // Calculate HMAC for integrity check
            byte[] hmacKey = Encoding.UTF8.GetBytes("Kelompok4@2023"); // Ganti dengan kunci HMAC yang aman
            HMac hmac = new HMac(new Sha256Digest());
            hmac.Init(new KeyParameter(hmacKey));
            hmac.BlockUpdate(encryptedBytes, 0, encryptedBytes.Length);
            byte[] hmacResult = new byte[hmac.GetMacSize()];
            hmac.DoFinal(hmacResult, 0);

            // Combine encrypted data and HMAC
            byte[] combinedData = new byte[encryptedBytes.Length + hmacResult.Length];
            Array.Copy(encryptedBytes, 0, combinedData, 0, encryptedBytes.Length);
            Array.Copy(hmacResult, 0, combinedData, encryptedBytes.Length, hmacResult.Length);

            return Convert.ToBase64String(combinedData);
        }



        public static string Decrypt(string encryptedText)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] ivBytes = Encoding.UTF8.GetBytes(initialitationValue);
            byte[] encryptedData = Convert.FromBase64String(encryptedText);

            // Extract HMAC
            int hmacSize = 32; // Panjang HMAC dalam byte
            byte[] hmac = new byte[hmacSize];
            Array.Copy(encryptedData, encryptedData.Length - hmacSize, hmac, 0, hmacSize);

            // Extract encrypted data
            byte[] encryptedBytes = new byte[encryptedData.Length - hmacSize];
            Array.Copy(encryptedData, 0, encryptedBytes, 0, encryptedData.Length - hmacSize);

            // Verify HMAC
            byte[] hmacKey = Encoding.UTF8.GetBytes("Kelompok4@2023"); // Ganti dengan kunci HMAC yang sama dengan yang digunakan dalam enkripsi
            HMac hmacAlgorithm = new HMac(new Sha256Digest());
            hmacAlgorithm.Init(new KeyParameter(hmacKey));
            hmacAlgorithm.BlockUpdate(encryptedBytes, 0, encryptedBytes.Length);
            byte[] calculatedHmac = new byte[hmacSize];
            hmacAlgorithm.DoFinal(calculatedHmac, 0);

            // Bandingkan HMAC yang dihitung dengan HMAC yang diterima
            for (int i = 0; i < hmacSize; i++)
            {
                if (calculatedHmac[i] != hmac[i])
                {
                    throw new InvalidOperationException("HMAC validation failed. Data may have been tampered.");
                }
            }

            IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS7Padding");
            KeyParameter keyParam = ParameterUtilities.CreateKeyParameter("AES", keyBytes);
            ParametersWithIV ivParam = new ParametersWithIV(keyParam, ivBytes);

            cipher.Init(false, ivParam);
            byte[] decryptedBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
            int length = cipher.ProcessBytes(encryptedBytes, decryptedBytes, 0);
            cipher.DoFinal(decryptedBytes, length);

            return Encoding.UTF8.GetString(decryptedBytes).TrimEnd('\0');
        }
    }
}
