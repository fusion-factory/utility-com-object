using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace FlowUtilities
{
    public class PGP
    {
        private int _returnCode;
        private string _returnMessage;
        private string _relativeFileName = "";

        public PGP()
        {
            CompressionAlgorithm = (int)enumCompressionAlgorithm.Uncompressed;
            SymmetricKeyAlgorithm = (int)enumSymmetricKeyAlgorithm.TripleDes;
            SignatureType = (int)PgpSignature.DefaultCertification;
            PublicKeyAlgorithm = (int)enumPublicKeyAlgorithm.RsaGeneral;
            FileType = (int)enumFileType.Text;
            EncryptionStrength = 4096;
            Certainty = 8;
        }

        #region Properties
        /// <summary>This property is the compression algorithm of the file to encrypt.  
        /// Use this when encrypting a file. Default value is 0 - Uncompressed.
        /// Valid values are: 1 - Zip, 2 - ZLib, 3 - BZip2
        /// </summary>
        public int CompressionAlgorithm
        {
            get;
            set;
        }

        /// <summary>This property is the algorith type for compressing the data using the public key provided.
        /// Use this when encrypting a file. Default value is 2 - Triple DES.
        /// Valid values are: 0 - Null, 1 - Idea, 2 - TripleDes, 3 - Cast5, 4 - Blowfish, 5 - Safer,
        /// 6 - Des, 7 - Aes128, 8 - Aes192, 9 - Aes256, 10 - Twofish, 11 - Camellia128,
        /// 12 - Camellia192, 13 - Camellia256
        /// </summary>
        public int SymmetricKeyAlgorithm
        {
            get;
            set;
        }

        /// <summary>This property a signature type that can represents a certification.  
        /// Use for generating secret key.  Default value is 16 - Default Certification.
        /// Valid values are: 16 - DefaultCertification, 17 - NoCertification, 
        /// 18 - CasualCertification, 19 - PositiveCertification
        /// </summary>
        public int SignatureType
        {
            get;
            set;
        }

        /// <summary>This property the algorithm use for generating keys.
        /// Default value is 1 - RSA General
        /// Valid values are: 1 - RSA General, 2 - RSA Encrypt, 3 - RSA Sign,
        /// 16 - ElGamal Encrypt, 17 - DSA, 18 - EC/ECDH, 19 - ECDsa,
        /// 20 - ElGamal General, 21- DiffieHellman, 100 - Experimental_1,
        /// 101 - Experimental 2, 102 - Experimental 3, 103 - Experimental 4,
        /// 104 - Experimental 5, 105 - Experimental 6, 106 - Experimental 7,
        /// 107 - Experimental 8, 108 - Experimental 9, 109 - Experimental 10,
        /// 110 - Experimental 11
        /// </summary>
        public int PublicKeyAlgorithm
        {
            get;
            set;
        }

        /// <summary>This property the algorithm use for generating keys.
        /// Default value is 1- Text
        /// Valid values are: 0 - Binary, 1 - Text, 2 - UTF8
        /// </summary>
        public int FileType
        {
            get;
            set;
        }

        /// <summary>This property is the strength of encryption for RSA type.  
        /// Use when generating public/private keys. Default value of 4096.
        /// </summary>
        public int EncryptionStrength
        {
            get;
            set;
        }

        /// <summary>This value is the validity value. Use when generating public/private keys. Default value of 8.
        /// </summary>
        public int Certainty
        {
            get;
            set;
        }
        #endregion

        #region Return Code / Messages
        /// <summary>This value is the return code after calling a function.
        /// Value codes are: 110 - Parameter input file is empty.
        /// 120 - Parameter output file is empty.
        /// 130 - Parameter public key is empty.
        /// 140 - Parameter private key is empty.
        /// 210 - Input file not found.
        /// 220 - Public key not found.
        /// 230 - Private key not found.
        /// 310 - Provide compression algorithm if parameter [isFileCompressed] is true. Valid values are: Zip = 1 = Zip, 2 = ZLib, 3 = BZip2.
        /// 320 - Invalid value for compression algorithm. Valid values are: Zip = 1 = Zip, 2 = ZLib, 3 = BZip2.
        /// 410 - Invalid value for file type. Valid values are: 0 = Binary, 1= Text, 2 = UTF8.
        /// 510 - Public key is not found in key rings.
        /// 610 - Secret key for message not found.
        /// 620 - Failed to Decrypt. Encrypted message contains signed message.
        /// 630 - Failed to Decrypt. Message is not a simple encrypted file.
        /// 800 - Error encrypting file.
        /// 999 - Process successful.
        /// </summary>
        public int ReturnCode()
        {
            return this._returnCode;
        }

        /// <summary>This value is the return message after calling a function.
        /// </summary>
        public string ReturnMessage()
        {
            return this._returnMessage;
        }

        private string SetMessage(int returnCode)
        {
            this._returnCode = returnCode;
            switch (returnCode)
            {
                case 110:
                    return "Parameter input file is empty.";
                case 120:
                    return "Parameter out file is empty.";
                case 130:
                    return "Parameter public key is empty.";
                case 140:
                    return "Parameter private key is empty.";
                case 210:
                    return "Input file not found.";
                case 220:
                    return "Public key not found.";
                case 230:
                    return "Private key not found.";
                case 310:
                    return "Provide compression algorithm if parameter [isFileCompressed] is true. Valid values are: Zip = 1 = Zip, 2 = ZLib, 3 = BZip2.";
                case 320:
                    return "Invalid value for compression algorithm. Valid values are: Zip = 1 = Zip, 2 = ZLib, 3 = BZip2.";
                case 410:
                    return "Invalid value for file type. Valid values are: 0 = Binary, 1= Text, 2 = UTF8.";
                case 510:
                    return "Public key is not found in key rings.";
                case 610:
                    return "Secret key for message not found.";
                case 620:
                    return "Failed to Decrypt. Encrypted message contains signed message.";
                case 630:
                    return "Failed to Decrypt. Message is not a simple encrypted file.";
                case 800:
                    return "Error encrypting file.";
                case 999:
                    return "Process successful.";
                default:
                    return "";
            }
        }
        #endregion

        #region Enums
        private enum enumReturnCode
        {
            ParameterInputFileIsEmpty = 110,
            ParameterOutFileIsEmpty = 120,
            ParameterPublicKeyIsEmpty = 130,
            ParameterPrivateKeyIsEmpty = 140,
            InputFileNotFound = 210,
            PublicKeyFileNotFound = 220,
            PrivateKeyFileNotFound = 230,
            NoCompressionAlgorithm = 310,
            CompressionAlgorithmOutOfRange = 320,
            FileTypeOutOfRange = 410,
            PublicKeyNotFoundInEncryptionRing = 510,
            SecretKeyForMessageNotFound = 610,
            CannotDecryptCozEncryptedMessageContainsSignedMessage = 620,
            CannotDecryptCozMessageIsNotASimpleEncryptedFile = 630,
            GeneralErrorEncrypt = 800,
            Success = 999
        }

        private enum enumCompressionAlgorithm
        {
            Uncompressed = 0,
            Zip = 1,
            ZLib = 2,
            BZip2 = 3
        }

        private enum enumSymmetricKeyAlgorithm
        {
            Null = 0,
            Idea = 1,
            TripleDes = 2,
            Cast5 = 3,
            Blowfish = 4,
            Safer = 5,
            Des = 6,
            Aes128 = 7,
            Aes192 = 8,
            Aes256 = 9,
            Twofish = 10,
            Camellia128 = 11,
            Camellia192 = 12,
            Camellia256 = 13
        }

        private enum enumPublicKeyAlgorithm
        {
            RsaGeneral = 1,
            RsaEncrypt = 2,
            RsaSign = 3,
            ElGamalEncrypt = 16,
            Dsa = 17,
            EC = 18,
            ECDH = 18,
            ECDsa = 19,
            ElGamalGeneral = 20,
            DiffieHellman = 21,
            Experimental_1 = 100,
            Experimental_2 = 101,
            Experimental_3 = 102,
            Experimental_4 = 103,
            Experimental_5 = 104,
            Experimental_6 = 105,
            Experimental_7 = 106,
            Experimental_8 = 107,
            Experimental_9 = 108,
            Experimental_10 = 109,
            Experimental_11 = 110
        }

        private enum enumFileType
        {
            Binary = 0,
            Text = 1,
            UTF8 = 2
        }
        #endregion

        #region Validation
        //Validation for FileEncrypt parameters
        private bool Valid(string inputFile, string outputFile, string publicKey)
        {
            if (!Valid(inputFile, outputFile))
            {
                return false;
            }

            if (String.IsNullOrEmpty(publicKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterPublicKeyIsEmpty);
                return false;
            }

            if (!File.Exists(publicKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.PublicKeyFileNotFound);
                return false;
            }
            return true;
        }

        //Common validation function
        private bool Valid(string inputFile, string outputFile)
        {
            if (String.IsNullOrEmpty(inputFile))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterInputFileIsEmpty);
                return false;
            }

            if (String.IsNullOrEmpty(outputFile))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterOutFileIsEmpty);
                return false;
            }

            if (!File.Exists(inputFile))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.InputFileNotFound);
                return false;
            }

            return true;
        }

        //Validation for FileDecrypt parameters
        private bool Valid(string inputFile, string outputFile, string publicKey, string privateKey)
        {
            if (!Valid(inputFile, outputFile))
            {
                return false;
            }

            //public key is optional, no need to validate public key

            if (String.IsNullOrEmpty(privateKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterPrivateKeyIsEmpty);
                return false;
            }

            //if public key is not empty, validate that file exists
            if (!String.IsNullOrEmpty(publicKey) && !File.Exists(publicKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.PublicKeyFileNotFound);
                return false;
            }

            if (!File.Exists(privateKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.PrivateKeyFileNotFound);
                return false;
            }

            return true;
        }
        #endregion

        #region Encrypt
        /// <summary>This method encrypts file using PGP public key.
        /// <param name="inputFile">the path and filename for the file to be encrypted. Mandatory parameter. The file must exists.</param>
        /// <param name="outputFile">the path and filename for the result encrypted file.  Preferred extension (.pgp). Mandatory paramater.</param>
        /// <param name="publicKey">the path and filename of the receipient's public key, with the (.asc) extension. Mandatory parameter.  The file must exists.</param>
        /// <param name="checkIntegrity">A boolean value of true to turn on integrity protection. Optional parameter. Default value is true.</param>
        /// </summary>
        public bool FileEncrypt(string inputFile, string outputFile, string publicKey, bool checkIntegrity = true)
        {
            try
            {
                if (!Valid(inputFile, outputFile, publicKey))
                    return false;

                using (Stream publicKeyStream = File.OpenRead(publicKey))
                {
                    using (Stream outputStream = File.Create(outputFile))
                    {
                        using (Stream inputStream = File.OpenRead(inputFile))
                        {
                            Encrypt(inputStream, outputStream, publicKeyStream, checkIntegrity);
                        }
                    }
                }

                this._returnMessage = SetMessage((int)enumReturnCode.Success);
                return true;
            }
            catch (Exception exc)
            {
                this._returnMessage = exc.Message;
                this._returnCode = (int)enumReturnCode.GeneralErrorEncrypt;
                return false;
            }
        }

        private void Encrypt(Stream inputStream, Stream outputStream, Stream publicKeyStream,
            bool checkIntegrity = true)
        {
            Stream pkStream = publicKeyStream;

            using (MemoryStream @out = new MemoryStream())
            {
                if (CompressionAlgorithm != (int)enumCompressionAlgorithm.Uncompressed)
                {
                    PgpCompressedDataGenerator comData = new PgpCompressedDataGenerator((CompressionAlgorithmTag)CompressionAlgorithm);
                    //WriteStream(comData.Open(@out), FileTypeToPgpChar((int)FileType), inputStream, GetFileName(inputStream));
                    WriteStream(comData.Open(@out), FileTypeToPgpChar((int)FileType), inputStream, GetRelativeFileName(inputStream));
                    comData.Close();
                }
                else
                {
                    //WriteStream(@out, FileTypeToPgpChar((int)FileType), inputStream, GetFileName(inputStream));
                    WriteStream(@out, FileTypeToPgpChar((int)FileType), inputStream, GetRelativeFileName(inputStream));
                }

                PgpEncryptedDataGenerator pk = new PgpEncryptedDataGenerator((SymmetricKeyAlgorithmTag)SymmetricKeyAlgorithm, checkIntegrity, new SecureRandom());
                pk.AddMethod(ReadPublicKey(pkStream));

                byte[] bytes = @out.ToArray();

                using (Stream plainStream = pk.Open(outputStream, bytes.Length))
                {
                    plainStream.Write(bytes, 0, bytes.Length);
                }
            }
        }
        #endregion

        #region Decrypt
        /// <summary>This method decrypts file using PGP private key with or without pass phrase.
        /// <param name="inputFile">the path and filename for the encrypted file to be decrypted, of extension (.pgp). Mandatory parameter. The file must exists.</param>
        /// <param name="outputFile">the path and filename for the result decrypted file.  Mandatory paramater.</param>
        /// <param name="privateKey">the path and filename of the receipient's private key, with the (.asc) extension. Mandatory parameter. The file must exists.</param>
        /// <param name="publicKey">the path and filename of the receipient's public key, with the (.asc) extension. Provide empty value if you do not want to check if the encryption key exists.</param>
        /// <param name="passPhrase">The password to decrypt and use your private key. Optional parameter. Default to empty.</param>
        /// </summary>
        public bool FileDecrypt(string inputFile, string outputFile, string privateKey, string publicKey = "", string passPhrase = "")
        {
            if (!Valid(inputFile, outputFile, publicKey, privateKey))
                return false;

            //ChoPGPEncryptionKeys encryptionKeys = new ChoPGPEncryptionKeys(publicKeyFilePath, privateKeyFilePath, passPhrase);
            //if (encryptionKeys == null)
            //    throw new ArgumentNullException("Encryption Key not found.");

            using (Stream inputStream = File.OpenRead(inputFile))
            {
                using (Stream privateKeyStream = File.OpenRead(privateKey))
                {
                    using (Stream outStream = File.Create(outputFile))
                        Decrypt(inputStream, outStream, privateKeyStream, passPhrase);
                }
            }

            this._returnMessage = SetMessage((int)enumReturnCode.Success);
            return true;
        }

        /// <summary>This method check the relative filename, for debugging purpose.
        /// </summary>
        public string GetRelativeFileName()
        {
            return this._relativeFileName;
        }

        private void Decrypt(Stream inputStream, Stream outputStream, Stream privateKeyStream, string passwordPhrase)
        {
            if (passwordPhrase == null)
                passwordPhrase = String.Empty;

            PgpObjectFactory objFactory = new PgpObjectFactory(PgpUtilities.GetDecoderStream(inputStream));
            //find secret key
            PgpSecretKeyRingBundle pgpSecretKey = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));

            PgpObject obj = null;
            if (objFactory != null)
                obj = objFactory.NextPgpObject();

            // the first object might be a PGP marker packet.
            PgpEncryptedDataList enc = null;
            if (obj is PgpEncryptedDataList)
                enc = (PgpEncryptedDataList)obj;
            else
                enc = (PgpEncryptedDataList)objFactory.NextPgpObject();

            //now decrypt
            PgpPrivateKey privateKey = null;
            PgpPublicKeyEncryptedData pbe = null;
            foreach (PgpPublicKeyEncryptedData pked in enc.GetEncryptedDataObjects())
            {
                privateKey = FindSecretKey(pgpSecretKey, pked.KeyId, passwordPhrase.ToCharArray());

                if (privateKey != null)
                {
                    pbe = pked;
                    break;
                }
            }

            if (privateKey == null)
            {
                this._returnMessage = SetMessage((int)enumReturnCode.SecretKeyForMessageNotFound);
                throw new ArgumentException(this._returnMessage);
            }

            PgpObjectFactory plainFact = null;

            using (Stream clear = pbe.GetDataStream(privateKey))
            {
                plainFact = new PgpObjectFactory(clear);
            }

            PgpObject message = plainFact.NextPgpObject();
            if (message is PgpOnePassSignatureList)
                message = plainFact.NextPgpObject();

            if (message is PgpCompressedData)
            {
                PgpCompressedData cData = (PgpCompressedData)message;
                PgpObjectFactory of = null;

                using (Stream compDataIn = cData.GetDataStream())
                {
                    of = new PgpObjectFactory(compDataIn);
                }

                message = of.NextPgpObject();
                if (message is PgpOnePassSignatureList)
                {
                    message = of.NextPgpObject();
                    PgpLiteralData Ld = null;
                    Ld = (PgpLiteralData)message;
                    Stream unc = Ld.GetInputStream();
                    Streams.PipeAll(unc, outputStream);
                }
                else
                {
                    PgpLiteralData Ld = null;
                    Ld = (PgpLiteralData)message;
                    Stream unc = Ld.GetInputStream();
                    Streams.PipeAll(unc, outputStream);
                }
            }
            else if (message is PgpLiteralData)
            {
                PgpLiteralData ld = (PgpLiteralData)message;
                string outFileName = ld.FileName;

                Stream unc = ld.GetInputStream();
                Streams.PipeAll(unc, outputStream);
            }
            else if (message is PgpOnePassSignatureList)
            {
                this._returnMessage = SetMessage((int)enumReturnCode.CannotDecryptCozEncryptedMessageContainsSignedMessage);
                throw new PgpException(this._returnMessage);
            }
            else
            {
                this._returnMessage = SetMessage((int)enumReturnCode.CannotDecryptCozMessageIsNotASimpleEncryptedFile);
                throw new PgpException(this._returnMessage);
            }
        }
        #endregion

        #region Public Keys / Private Keys / Secret Key
        private PgpPublicKey ReadPublicKey(Stream inputStream)
        {
            inputStream = PgpUtilities.GetDecoderStream(inputStream);

            PgpPublicKeyRingBundle pgpPub = new PgpPublicKeyRingBundle(inputStream);

            //loop through the key rings.
            foreach (PgpPublicKeyRing kRing in pgpPub.GetKeyRings())
            {
                List<PgpPublicKey> keys = kRing.GetPublicKeys()
                    .Cast<PgpPublicKey>()
                    .Where(k => k.IsEncryptionKey).ToList();

                const int encryptKeyFlags = PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage;

                foreach (PgpPublicKey key in keys.Where(k => k.Version >= 4 && !k.IsMasterKey))
                {
                    foreach (PgpSignature s in key.GetSignatures())
                    {
                        if (s.GetHashedSubPackets().GetKeyFlags() == encryptKeyFlags)
                            return key;
                    }
                }

                if (keys.Any())
                    return keys.First();
            }

            this._returnMessage = SetMessage((int)enumReturnCode.PublicKeyNotFoundInEncryptionRing);
            throw new Exception(this._returnMessage);
        }

        /// <summary>This method generates PGP public key file and private key file
        /// <param name="publicKey">the path and filename for the public key to be generated, having file extension (.asc). Mandatory parameter.</param>
        /// <param name="privateKey">the path and filename for the private key to be generated, having file extension (.asc). Mandatory parameter.</param>
        /// <param name="username">the username of the owner of the public/private key. Can also be identity. Mandatory parameter.</param>
        /// <param name="password">the password of the owner of the public/private key. Can also be pass phrase. Optional parameter. Dafault value of empty string.</param>
        /// </summary>
        public bool GenerateKey(string publicKey, string privateKey, string username = "", string password = "")
        {
            if (String.IsNullOrEmpty(publicKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterPublicKeyIsEmpty);
                return false;
            }

            if (String.IsNullOrEmpty(privateKey))
            {
                this._returnMessage = SetMessage((int)enumReturnCode.ParameterPrivateKeyIsEmpty);
                return false;
            }

            using (Stream streamPublicKey = File.Open(publicKey, FileMode.OpenOrCreate))
            using (Stream streamPrivateKey = File.Open(privateKey, FileMode.OpenOrCreate))
                GenerateKey(streamPublicKey, streamPrivateKey, username, password, this.EncryptionStrength, this.Certainty);

            this._returnMessage = SetMessage((int)enumReturnCode.Success);
            return true;
        }

        private void GenerateKey(Stream publicKeyStream, Stream privateKeyStream, string userName = null, string password = null, int strength = 1024, int certainty = 8, bool armor = true)
        {
            userName = userName == null ? string.Empty : userName;
            password = password == null ? string.Empty : password;

            IAsymmetricCipherKeyPairGenerator kpg = new RsaKeyPairGenerator();
            kpg.Init(new RsaKeyGenerationParameters(BigInteger.ValueOf(0x13), new SecureRandom(), strength, certainty));
            AsymmetricCipherKeyPair kp = kpg.GenerateKeyPair();

            ExportKeyPair(privateKeyStream, publicKeyStream, kp.Public, kp.Private, userName, password.ToCharArray(), armor);
        }

        private void ExportKeyPair(Stream secretOut, Stream publicOut, AsymmetricKeyParameter publicKey,
                    AsymmetricKeyParameter privateKey, string identity, char[] passPhrase, bool armor)
        {
            if (secretOut == null)
                throw new ArgumentException("secretOut");

            if (publicOut == null)
                throw new ArgumentException("publicOut");

            if (armor)
            {
                secretOut = new ArmoredOutputStream(secretOut);
            }

            PgpSecretKey secretKey = new PgpSecretKey(
                SignatureType,
                (PublicKeyAlgorithmTag)(int)PublicKeyAlgorithm,
                publicKey,
                privateKey,
                DateTime.UtcNow,
                identity,
                (SymmetricKeyAlgorithmTag)(int)SymmetricKeyAlgorithm,
                passPhrase,
                null,
                null,
                new SecureRandom()
                );

            secretKey.Encode(secretOut);

            secretOut.Close();

            if (armor)
            {
                publicOut = new ArmoredOutputStream(publicOut);
            }

            PgpPublicKey key = secretKey.PublicKey;

            key.Encode(publicOut);

            publicOut.Close();
        }

        private PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] password)
        {
            PgpSecretKey pgpSecKey = pgpSec.GetSecretKey(keyId);

            if (pgpSecKey == null)
                return null;

            return pgpSecKey.ExtractPrivateKey(password);
        }
        #endregion

        #region other private methods
        private void WriteStream(Stream outputStream, char fileType, Stream inputStream, string fileName)
        {
            PgpLiteralDataGenerator data = new PgpLiteralDataGenerator();
            Stream streamOut = data.Open(outputStream, fileType, fileName, inputStream.Length, DateTime.Now);
            //StreamContents(inputStream, streamOut, bufferSize);
            StreamContents(inputStream, streamOut, this.EncryptionStrength);
        }

        private void StreamContents(Stream input, Stream streamOut, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];

            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                streamOut.Write(buffer, 0, len);
            }
        }

        private string GetFileName(Stream stream)
        {
            if (stream == null || !(stream is FileStream))
                return "name";

            return ((FileStream)stream).Name;
        }

        private string GetRelativeFileName(Stream stream)
        {
            if (stream == null || !(stream is FileStream))
                return "name";

            this._relativeFileName = Path.GetFileName(((FileStream)stream).Name);
            return this._relativeFileName;
        }

        private char FileTypeToPgpChar(int fileType)
        {
            if (fileType == (int)enumFileType.UTF8)
                return PgpLiteralData.Utf8;
            else if (fileType == (int)enumFileType.Text)
                return PgpLiteralData.Text;
            else
                return PgpLiteralData.Binary;
        }
        #endregion
    }
}
