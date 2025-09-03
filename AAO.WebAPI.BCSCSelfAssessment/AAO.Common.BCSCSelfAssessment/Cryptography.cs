namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Cryptography
    {
        #region Private Constants
        public const string encryptionKey = "SS@ERef*%(i7q1d!";
        private const string C_SALTVALUE = "956565.Sefser1sdf><;ophthalmology?+|-";  // Any valid string
        private const string C_PASSPHRASE = "<Sachin><test><Stopf*..!";
        private const int passwordIterations = 2;
        private const int keySize = 128;
        private const string hashAlgorithm = "SHA1";
        #endregion

        #region constructor
        private Cryptography()
        {
            // TODO: Add constructor logic here
        }
        #endregion

        #region Encryption function

        /// <summary>
        /// Encrypts specified Id using Rijndael symmetric key algorithm
        /// </summary>
        /// <param name="StringToEncrypt">Id to be Encrypted.</param>
        /// <returns>Returns Encrypted Id.</returns>
        public static string Encrypt(string IdToEncrypt)
        {
            return Encrypt(IdToEncrypt, encryptionKey);
        }

        /// <summary>
        /// Encrypts specified plain text string using Rijndael symmetric key algorithm
        /// </summary>
        /// <param name="StringToEncrypt">Plain text value to be Encrypted.</param>
        /// <param name="Key"> Key for encryption</param>
        /// <returns>Returns Encrypted string value.</returns>
        public static string Encrypt(string StringToEncrypt, string Key)
        {
            ICryptoTransform encryptor;
            byte[] initVectorBytes;
            byte[] saltValueBytes;
            PasswordDeriveBytes password;
            byte[] plainTextBytes;
            byte[] keyBytes;
            RijndaelManaged symmetricKey;
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            byte[] bytEncryptedText;
            string strEncryptedText;

            try
            {
                if (Key.Length != 16)
                {
                    throw new Exception("Key value must be 16 character long");
                }

                if (StringToEncrypt == null || StringToEncrypt.Length == 0)
                {
                    throw new Exception("String to be encrypted should be specified");
                }

                // Convert strings into byte arrays.
                // Assumes that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8 encoding.
                initVectorBytes = Encoding.ASCII.GetBytes(Key);

                saltValueBytes = Encoding.ASCII.GetBytes(C_SALTVALUE);

                // Convert plaintext into a byte array.
                // Assumes that plaintext contains UTF8-encoded characters.
                plainTextBytes = Encoding.UTF8.GetBytes(StringToEncrypt);

                // First, we must create a password, from which the key will be derived.
                // This password will be generated from the specified passphrase and
                // salt value. The password will be created using the specified hash
                // algorithm. Password creation can be done in several iterations.
                password = new PasswordDeriveBytes(C_PASSPHRASE, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                keyBytes = password.GetBytes((int)keySize / 8);

                // Create uninitialized Rijndael encryption object.
                symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

                // Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert our encrypted data from a memory stream into a byte array.
                bytEncryptedText = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert encrypted data into a base64-encoded string.
                strEncryptedText = Convert.ToBase64String(bytEncryptedText);

                // Return encrypted string.
                return strEncryptedText;
            }
            catch (Exception exception)
            {
                throw (exception);
            }
        }
        #endregion

        #region Decryption function

        /// <summary>
        /// Decrypts specified encrypted Id using Rijndael symmetric key algorithm
        /// </summary>
        /// <param name="StringToDecrypt">Encrypted Id to be Decrypted.</param>
        /// <returns>Returns Decrypted Id.</returns>
        public static string Decrypt(string IdToDecrypt)
        {
            string encryptedId = SanitizeEncryptedId(IdToDecrypt);
            return Decrypt(encryptedId, encryptionKey);
        }

        public static string DecryptPassword(string encryptedPassword, string Key)
        {
            string encryptedId = SanitizeEncryptedId(encryptedPassword);
            return Decrypt(encryptedId, Key);
        }

        /// <summary>
        /// Decrypts specified encrypted string using Rijndael symmetric key algorithm
        /// </summary>
        /// <param name="StringToDecrypt">Encrypted text value to be Decrypted.</param>
        /// <param name="Key"> Key for decryption</param>
        /// <returns>Returns Decrypted string value.</returns>
        public static string Decrypt(string StringToDecrypt, string Key)
        {
            ICryptoTransform decryptor;
            byte[] initVectorBytes;
            byte[] saltValueBytes;
            PasswordDeriveBytes password;
            byte[] plainTextBytes;
            byte[] keyBytes;
            RijndaelManaged symmetricKey;
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            byte[] bytEncryptedText;
            string strUnencryptedText;
            int decryptedByteCount;

            try
            {
                if (Key.Length != 16)
                {
                    throw new Exception("Key value must be 16 character long");
                }

                if (StringToDecrypt == null || StringToDecrypt.Length == 0)
                {
                    throw new Exception("String to be decrypted is empty");
                }

                // Convert strings defining encryption key characteristics into byte
                // arrays. Assumes that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                initVectorBytes = Encoding.ASCII.GetBytes(Key);
                saltValueBytes = Encoding.ASCII.GetBytes(C_SALTVALUE);

                // Convert our ciphertext into a byte array.
                bytEncryptedText = Convert.FromBase64String(StringToDecrypt);

                // First, we must create a password, from which the key will be 
                // derived. This password will be generated from the specified 
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                password = new PasswordDeriveBytes(C_PASSPHRASE, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                keyBytes = password.GetBytes((int)keySize / 8);

                // Create uninitialized Rijndael encryption object.
                symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization 
                // vector. Key size will be defined based on the number of the key 
                // bytes.
                decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                memoryStream = new MemoryStream(bytEncryptedText);

                // Define memory stream which will be used to hold encrypted data.
                cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don//t know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                plainTextBytes = new byte[bytEncryptedText.Length];

                // Start decrypting.
                try
                {
                    decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    // Convert decrypted data into a string. 
                    // Let us assume that the original plaintext string was UTF8-encoded.
                    strUnencryptedText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                    cryptoStream.Close();
                }
                catch
                {
                    strUnencryptedText = "";
                }

                memoryStream.Close();

                return strUnencryptedText;
            }
            catch (Exception err)
            {
                throw (err);
            }
        }

        /// <summary>
        /// To return valid querystring value
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string SanitizeEncryptedId(string encryptedId)
        {
            string lowerCaseId = encryptedId
                .Replace("%3C", "%3c")
                .Replace("%3E", "%3e")
                .Replace("%7B", "%7b")
                .Replace("%7D", "%7d")
                .Replace("%7C", "%7c")
                .Replace("%5C", "%5c")
                .Replace("%5E", "%5e")
                .Replace("%7E", "%7e")
                .Replace("%5B", "%5b")
                .Replace("%5D", "%5d")
                .Replace("%3B", "%3b")
                .Replace("%2F", "%2f")
                .Replace("%3F", "%3f")
                .Replace("%3A", "%3a")
                .Replace("%3D", "%3d")
                .Replace("%2B", "%2b");

            string result = lowerCaseId;
            result = result
                .Replace("%25", "%")
                .Replace("%3c", "<")
                .Replace("%3e", ">")
                .Replace("%23", "#")
                .Replace("%7b", "{")
                .Replace("%7d", "}")
                .Replace("%7c", "|")
                .Replace("%5c", @"\")
                .Replace("%5e", "^")
                .Replace("%7e", "~")
                .Replace("%5b", "[")
                .Replace("%5d", "]")
                .Replace("%60", "`")
                .Replace("%3b", ";")
                .Replace("%2f", "/")
                .Replace("%3f", "?")
                .Replace("%3a", ":")
                .Replace("%40", "@")
                .Replace("%3d", "=")
                .Replace("%26", "&")
                .Replace("%24", "$")
                .Replace("%2b", "+")
                .Replace(" ", "+");
            return result;
        }

        #endregion
    }
}
