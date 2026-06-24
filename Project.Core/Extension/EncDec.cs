using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Project.Core.Extension
{
    public class EncDec
    {
        #region Private members

        private static readonly string _constantText = "R#$g5H%H89I#@YH6";
        private static string _initVector = string.Empty;
        private static readonly string _dekCurrentKey = "R#$g5H%H89I#@YH6";
        // private static string KEKCurrentKey = "@O7K%90%O#F4eT71";
        private static readonly int _dekCurrentVersion = 0;
        private static string _dekStringXml = string.Empty;
        //  If hashing algorithm is not specified, use SHA-1.
        private static readonly string _defaultHashAlgorithm = "SHA1";
        //  If key size is not specified, use the longest 256-bit key.
        private static readonly int _defaultKeySize = 256;
        //  Do not allow salt to be longer than 255 bytes, because we have only
        //  1 byte to store its length. 
        // private static int MAX_ALLOWED_SALT_LEN = 255;

        //  Do not allow salt to be smaller than 4 bytes, because we use the first
        //  4 bytes of salt to store its length. 
        private static readonly int _minAllowedSaltLen = 4;

        //  Random salt value will be between 4 and 8 bytes long.
        private static readonly int _defaultMinSaltLen = _minAllowedSaltLen;
        private const int DefaultMaxSaltLen = 8;

        //  Use these members to save min and max salt lengths.
        // private int minSaltLen = -1;
        // private int maxSaltLen = -1;

        //  These members will be used to perform encryption and decryption.
        private static ICryptoTransform encryptor = null;
        private static ICryptoTransform decryptor = null;

        public static int GetCurrentEncKeyVerions
        {
            get
            {
                return _dekCurrentVersion;
            }
        }

        #endregion

        #region Constructors

        static EncDec()
        {


        }
        #endregion

        #region Encryption routines

        // / <summary>
        // / Encrypt used publicly.
        // / </summary>
        // / <param name="plainText"></param>
        // / <param name="keyVersion"></param>
        // / <returns>Return encrypted value.</returns>
        public static string Encrypt(string plainText, string keyVersion = "")
        {
            return Encrypt(Encoding.UTF8.GetBytes(plainText), keyVersion);
        }

        // / <summary>
        // / Decrypt.
        // / </summary>
        // / <param name="cipherText">string.</param>
        // / <param name="keyVersion">Pass key version.</param>
        // / <returns>Return decrypted data.</returns>
        public static string Decrypt(string cipherText, string keyVersion = "")
        {
            cipherText = cipherText.Replace(" ", "+");
            return Decrypt(Convert.FromBase64String(cipherText), keyVersion);
        }

        // / <summary>
        // / Encrypt used private.
        // / </summary>
        // / <param name="plainTextBytes"></param>
        // / <param name="keyVersion"></param>
        // / <returns>Return encrypted value.</returns>
        private static string Encrypt(byte[] plainTextBytes, string keyVersion)
        {

            string str = Convert.ToBase64String(EncryptToBytes(plainTextBytes, keyVersion));
            return Convert.ToBase64String(EncryptToBytes(plainTextBytes, keyVersion));
        }

        // / <summary>
        // / Encrypt To Bytes.
        // / </summary>
        // / <param name="plainText"></param>
        // / <param name="keyVersion"></param>
        // / <returns>Return bytes.</returns>
        private static byte[] EncryptToBytes(string plainText, string keyVersion)
        {
            return EncryptToBytes(Encoding.UTF8.GetBytes(plainText), keyVersion);
        }

        // / <summary>
        // / Encrypt To Bytes.
        // / </summary>
        // / <param name="plainTextBytes">byte[]</param>
        // / <param name="keyVersion">string</param>
        // / <returns>Bytes</returns>
        private static byte[] EncryptToBytes(byte[] plainTextBytes, string keyVersion)
        {
            byte[] initVectorBytes = null;

            //  Salt used for password hashing (to generate the key, not during
            //  encryption) converted to a byte array.
            byte[] saltValueBytes = null;

            _initVector = _constantText;
            //  Get bytes of initialization vector.
            if (_initVector == null)
                initVectorBytes = new byte[0];
            else
                initVectorBytes = Encoding.ASCII.GetBytes(_initVector);

            saltValueBytes = new byte[0];
            string tempKey = string.Empty;

            if (keyVersion == string.Empty || keyVersion == "1")
                tempKey = _constantText;

            //  Generate password, which will be used to derive the key.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                       tempKey,
                                                       saltValueBytes,
                                                       _defaultHashAlgorithm,
                                                       2);

            //  Convert key to a byte array adjusting the size from bits to bytes.
            byte[] keyBytes = password.GetBytes(_defaultKeySize / 8);

            //  Initialize Rijndael key object.
            AesManaged symmetricKey = new AesManaged();

            //  If we do not have initialization vector, we cannot use the CBC mode.
            //  The only alternative is the ECB mode (which is not as good).
            if (initVectorBytes.Length == 0)
                symmetricKey.Mode = CipherMode.ECB;
            else
                symmetricKey.Mode = CipherMode.CBC;

            //  Create encryptor and decryptor, which we will use for cryptographic
            //  operations.
            encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);


            //  Add salt at the beginning of the plain text bytes (if needed).
            byte[] plainTextBytesWithSalt = AddSalt(plainTextBytes);

            //  Encryption will be performed using memory stream.
            MemoryStream memoryStream = new MemoryStream();

            //  Let's make cryptographic operations thread-safe.           
            //  To perform encryption, we must use the Write mode.
            CryptoStream cryptoStream = new CryptoStream(
                                               memoryStream,
                                               encryptor,
                                                CryptoStreamMode.Write);

            //  Start encrypting data.
            cryptoStream.Write(plainTextBytesWithSalt,
                                0,
                               plainTextBytesWithSalt.Length);

            //  Finish the encryption operation.
            cryptoStream.FlushFinalBlock();

            //  Move encrypted data from memory into a byte array.
            byte[] cipherTextBytes = memoryStream.ToArray();

            //  Close memory streams.
            memoryStream.Close();
            cryptoStream.Close();

            //  Return encrypted data.
            return cipherTextBytes;
        }
        #endregion

        #region Decryption routines


        // / <summary>
        // / Decrypt byte array by keyversion
        // / </summary>
        // / <param name="cipherTextBytes">byte[]</param>
        // / <param name="KeyVersion">string</param>
        // / <returns>return string</returns>
        private static string Decrypt(byte[] cipherTextBytes, string KeyVersion)
        {
            return Encoding.UTF8.GetString(DecryptToBytes(cipherTextBytes, KeyVersion));
        }


        private static byte[] DecryptToBytes(string cipherText, string KeyVersion)
        {
            return DecryptToBytes(Convert.FromBase64String(cipherText), KeyVersion);
        }


        private static byte[] DecryptToBytes(byte[] cipherTextBytes, string KeyVersion)
        {

            byte[] initVectorBytes = null;

            //  Salt used for password hashing (to generate the key, not during
            //  encryption) converted to a byte array.
            byte[] saltValueBytes = null;

            _initVector = _constantText;
            //  Get bytes of initialization vector.
            if (_initVector == null)
                initVectorBytes = new byte[0];
            else
                initVectorBytes = Encoding.ASCII.GetBytes(_initVector);

            //  Get bytes of salt (used in hashing).            
            saltValueBytes = new byte[0];
            string tempKey = string.Empty;
            if (KeyVersion == string.Empty || KeyVersion == "1")
                tempKey = _dekCurrentKey;

            //  Generate password, which will be used to derive the key.
            PasswordDeriveBytes password = new PasswordDeriveBytes(
                                                       tempKey,
                                                       saltValueBytes,
                                                       _defaultHashAlgorithm,
                                                       2);

            //  Convert key to a byte array adjusting the size from bits to bytes.
            byte[] keyBytes = password.GetBytes(_defaultKeySize / 8);

            //  Initialize Rijndael key object.
            AesManaged symmetricKey = new AesManaged();

            //  If we do not have initialization vector, we cannot use the CBC mode.
            //  The only alternative is the ECB mode (which is not as good).
            if (initVectorBytes.Length == 0)
                symmetricKey.Mode = CipherMode.ECB;
            else
                symmetricKey.Mode = CipherMode.CBC;

            //  Create encryptor and decryptor, which we will use for cryptographic
            //  operations.
            //  encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

            byte[] decryptedBytes = null;
            byte[] plainTextBytes = null;
            int decryptedByteCount = 0;
            int saltLen = 0;

            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

            //  Since we do not know how big decrypted value will be, use the same
            //  size as cipher text. Cipher text is always longer than plain text
            //  (in block cipher encryption), so we will just use the number of
            //  decrypted data byte after we know how big it is.
            decryptedBytes = new byte[cipherTextBytes.Length];

            //  Let's make cryptographic operations thread-safe.
            // lock (this)
            // {
            //  To perform decryption, we must use the Read mode.
            CryptoStream cryptoStream = new CryptoStream(
                                               memoryStream,
                                               decryptor,
                                               CryptoStreamMode.Read);

            //  Decrypting data and get the count of plain text bytes.
            decryptedByteCount = cryptoStream.Read(decryptedBytes,
                                                    0,
                                                    decryptedBytes.Length);
            //  Release memory.
            memoryStream.Close();
            cryptoStream.Close();
            // }

            //  If we are using salt, get its length from the first 4 bytes of plain
            //  text data.
            if (DefaultMaxSaltLen > 0 && DefaultMaxSaltLen >= _defaultMinSaltLen)
            {
                saltLen = (decryptedBytes[0] & 0x03) |
                            (decryptedBytes[1] & 0x0c) |
                            (decryptedBytes[2] & 0x30) |
                            (decryptedBytes[3] & 0xc0);
            }

            //  Allocate the byte array to hold the original plain text (without salt).
            plainTextBytes = new byte[decryptedByteCount - saltLen];

            //  Copy original plain text discarding the salt value if needed.
            Array.Copy(decryptedBytes, saltLen, plainTextBytes,
                        0, decryptedByteCount - saltLen);

            //  Return original plain text value.
            return plainTextBytes;
        }

        // / <summary>
        // / take any string and encrypt it using SHA1 then
        // / return the encrypted data
        // / </summary>
        // / <param name="data">input text you will enterd to encrypt it</param>
        // / <returns>return the encrypted text as hexadecimal string</returns>
        public static string GetSHA1HashData(string data)
        {
            // create new instance of SHA1
            SHA1 sha1 = SHA1.Create();

            // convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            // create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            // loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            //  return hexadecimal string
            return returnValue.ToString();
        }
        // / <summary>
        // / take any string and encrypt it using SHA256 then
        // / return the encrypted data
        // / </summary>
        // / <param name="data">input text you will enterd to encrypt it</param>
        // / <returns>return the encrypted text as hexadecimal string</returns>
        public static string GetSHA256HashData(string data)
        {
            // create new instance of SHA1
            SHA256 sha256 = SHA256.Create();

            // convert the input text to array of bytes
            byte[] hashData = sha256.ComputeHash(Encoding.Default.GetBytes(data));

            // create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            // loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            //  return hexadecimal string
            return returnValue.ToString();
        }
        // / <summary>
        // / take any string and encrypt it using MD5 then
        // / return the encrypted data 
        // / </summary>
        // / <param name="data">input text you will enterd to encrypt it</param>
        // / <returns>return the encrypted text as hexadecimal string</returns>
        public static string GetMD5HashData(string data)
        {
            // create new instance of md5
            MD5 md5 = MD5.Create();

            // convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            // create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            // loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            //  return hexadecimal string
            return returnValue.ToString();

        }
        #endregion

        #region Helper functions
        // / <summary>
        // / Adds an array of randomly generated bytes at the beginning of the
        // / array holding original plain text value.
        // / </summary>
        // / <param name="plainTextBytes">
        // / Byte array containing original plain text value.
        // / </param>
        // / <returns>
        // / Either original array of plain text bytes (if salt is not used) or a
        // / modified array containing a randomly generated salt added at the 
        // / beginning of the plain text bytes. 
        // / </returns>
        private static byte[] AddSalt(byte[] plainTextBytes)
        {
            //  The max salt value of 0 (zero) indicates that we should not use 
            //  salt. Also do not use salt if the max salt value is smaller than
            //  the min value.
            // if (maxSaltLen == 0 || maxSaltLen < minSaltLen)
            //     return plainTextBytes;

            //  Generate the salt.
            byte[] saltBytes = GenerateSalt();

            //  Allocate array which will hold salt and plain text bytes.
            byte[] plainTextBytesWithSalt = new byte[plainTextBytes.Length +
                                                     saltBytes.Length];
            //  First, copy salt bytes.
            Array.Copy(saltBytes, plainTextBytesWithSalt, saltBytes.Length);

            //  Append plain text bytes to the salt value.
            Array.Copy(plainTextBytes, 0,
                        plainTextBytesWithSalt, saltBytes.Length,
                        plainTextBytes.Length);

            return plainTextBytesWithSalt;
        }

        // / <summary>
        // / Generates an array holding cryptographically strong bytes.
        // / </summary>
        // / <returns>
        // / Array of randomly generated bytes.
        // / </returns>
        // / <remarks>
        // / Salt size will be defined at random or exactly as specified by the
        // / minSlatLen and maxSaltLen parameters passed to the object constructor.
        // / The first four bytes of the salt array will contain the salt length
        // / split into four two-bit pieces.
        // / </remarks>
        private static byte[] GenerateSalt()
        {
            //  We don't have the length, yet.
            int saltLen = 0;

            //  If min and max salt values are the same, it should not be random.
            // if (minSaltLen == maxSaltLen)
            //     saltLen = minSaltLen;
            // //  Use random number generator to calculate salt length.
            // else
            saltLen = GenerateRandomNumber(_defaultMinSaltLen, DefaultMaxSaltLen);

            //  Allocate byte array to hold our salt.
            byte[] salt = new byte[saltLen];

            //  Populate salt with cryptographically strong bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

            rng.GetNonZeroBytes(salt);

            //  Split salt length (always one byte) into four two-bit pieces and
            //  store these pieces in the first four bytes of the salt array.
            salt[0] = (byte)((salt[0] & 0xfc) | (saltLen & 0x03));
            salt[1] = (byte)((salt[1] & 0xf3) | (saltLen & 0x0c));
            salt[2] = (byte)((salt[2] & 0xcf) | (saltLen & 0x30));
            salt[3] = (byte)((salt[3] & 0x3f) | (saltLen & 0xc0));

            return salt;
        }

        // / <summary>
        // / Generates random integer.
        // / </summary>
        // / <param name="minValue">
        // / Min value (inclusive).
        // / </param>
        // / <param name="maxValue">
        // / Max value (inclusive).
        // / </param>
        // / <returns>
        // / Random integer value between the min and max values (inclusive).
        // / </returns>
        // / <remarks>
        // / This methods overcomes the limitations of .NET Framework's Random
        // / class, which - when initialized multiple times within a very short
        // / period of time - can generate the same "random" number.
        // / </remarks>
        private static int GenerateRandomNumber(int minValue, int maxValue)
        {
            //  We will make up an integer seed from 4 bytes of this array.
            byte[] randomBytes = new byte[4];

            //  Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            //  Convert four random bytes into a positive integer value.
            int seed = ((randomBytes[0] & 0x7f) << 24) |
                        (randomBytes[1] << 16) |
                        (randomBytes[2] << 8) |
                        (randomBytes[3]);

            //  Now, this looks more like real randomization.
            Random random = new Random(seed);

            //  Calculate a random number.
            return random.Next(minValue, maxValue + 1);
        }

        public static string GetHashing(string CCNumber)
        {
            string retHashing = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(CCNumber))
                {
                    CCNumber = CCNumber.Trim();

                    string constanttextLocal = _constantText;
                    string password = CCNumber;

                    string CCFirst = password.Substring(0, 1);
                    string CCLast = password.Substring(password.Length - 1);
                    int i = 2;

                    if (CCNumber.Length > 10)
                    {
                        i = 9;
                    }

                    long sumAll = Convert.ToInt32(password.Substring(password.Length - i));

                    sumAll = sumAll >> 4;

                    string salt = (Convert.ToInt64(sumAll / 9 * Convert.ToInt32(CCFirst) * Convert.ToInt32(CCLast))).ToString();

                    password = constanttextLocal.Substring(0, 3) + password + constanttextLocal.Substring(constanttextLocal.Length - 3);

                    byte[] array = Encoding.ASCII.GetBytes(salt);
                    string passwordHashSha256 = SimpleHash.ComputeHash(password, "SHA256", array);
                    string Finalhashing = passwordHashSha256.Length > 48 ? passwordHashSha256.Substring(0, 48) : passwordHashSha256;
                    retHashing = Finalhashing;
                }
                return retHashing;
            }
            catch (Exception ex)
            {
                var exception = ex.ToString();
                return retHashing;
            }
        }
        #endregion
    }
}
