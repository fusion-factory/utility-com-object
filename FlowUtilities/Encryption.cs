using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace FlowUtilities {
	/// <summary>
	/// Methods to perform Encryption 
	/// </summary>
	public class Encryption {
        // function to safely convert various flow strings/blobs into Byte[]
        private Byte[] AsBytes(Object o)
        {
            // empty string in Flow gets passed to COM as null
            // empty Blob in Flow gets pased to COM as DBNull
            if (o == null || o.GetType() == typeof(DBNull))
                return Encoding.UTF8.GetBytes("");

            // other Blob in Flow gets passed to COM as Byte[]
            if (o.GetType() == typeof(Byte[]))
                return (Byte[]) o;

            // otherwise Flow string gets passed as String
            // Since .Net strings are always UTF-16 , there is no guaranteed correct conversion for flow string to Byte[]
            // assume ASCII or UTF-8
            // to ensure binary data is honoured, use CreateBlobValue() in Flow
            if (o.GetType() == typeof(String))
                return Encoding.UTF8.GetBytes((string)o);

            // unknown type, cannot process
            return null;
         }
		
		#region Regular Hashes with immediate Base64 bit encoding
		/// <summary>
		/// Returns MD5 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string MD5Hash(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA1 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA1Hash(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA256 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA256Hash(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA384 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA384Hash(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(new SHA384CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA512 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA512Hash(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(new SHA512CryptoServiceProvider().ComputeHash(bytText));
		}
        #endregion

        #region Regular Hashes with each hashed bit converted to hexadecimal string
        /// <summary>
        /// Returns MD5 Hash with each hashed bit converted to hexadecimal string
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string MD5HashHex(Object textToHash)
        {
            byte[] bytText = AsBytes(textToHash);
            return ByteArrayToHexadecimalString(new MD5CryptoServiceProvider().ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA1 Hash with each hashed bit converted to hexadecimal string
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string SHA1HashHex(Object textToHash)
        {
            byte[] bytText = AsBytes(textToHash);
            return ByteArrayToHexadecimalString(new SHA1CryptoServiceProvider().ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA256 Hash with each hashed bit converted to hexadecimal string
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string SHA256HashHex(Object textToHash)
        {
            byte[] bytText = AsBytes(textToHash);
            return ByteArrayToHexadecimalString(new SHA256CryptoServiceProvider().ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA384 Hash with each hashed bit converted to hexadecimal string
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string SHA384HashHex(Object textToHash)
        {
            byte[] bytText = AsBytes(textToHash);
            return ByteArrayToHexadecimalString(new SHA384CryptoServiceProvider().ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA512 Hash with each hashed bit converted to hexadecimal string
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string SHA512HashHex(Object textToHash)
        {
            byte[] bytText = AsBytes(textToHash);
            return ByteArrayToHexadecimalString(new SHA512CryptoServiceProvider().ComputeHash(bytText));
        }
        #endregion

        #region Regular Hashes with each hashed bit converted to hexadecimal string, then Base64 bit encoded
        /// <summary>
        /// Returns MD5 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        public string MD5HashHexBase64(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(new MD5CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA1 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA1HashHexBase64(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(new SHA1CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA256 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA256HashHexBase64(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(new SHA256CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA384 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA384HashHexBase64(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(new SHA384CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA512 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA512HashHexBase64(Object textToHash){
			byte[] bytText=AsBytes(textToHash);
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(ByteArrayToHexadecimalString(new SHA512CryptoServiceProvider().ComputeHash(bytText))));
		}
		#endregion


		#region Keyed hashes
		/// <summary>
		/// Returns MD5 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		/// <param name="key">Cryptographic key</param>
		public string HmacMD5(Object textToHash, Object key){
			byte[] bytText=AsBytes(textToHash);
			byte[] bytKey=AsBytes(key);
			return Convert.ToBase64String(new HMACMD5(bytKey).ComputeHash(bytText));
			//byte[] hash=  new HMACMD5(bytKey).ComputeHash(bytText);
			//StringBuilder stringBuilder =  new StringBuilder();
			//foreach(byte b in hash){
			//	stringBuilder.Append(b.ToString("x2"));
			//}
			//return stringBuilder.ToString();
		}
		/// <summary>
		/// Returns SHA1 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		/// <param name="key">Cryptographic key</param>
		public string HmacSHA1(Object textToHash, Object key){
			byte[] bytText=AsBytes(textToHash);
			byte[] bytKey=AsBytes(key);
			return Convert.ToBase64String(new HMACSHA1(bytKey).ComputeHash(bytText));
			//byte[] hash=  new HMACSHA1(bytKey).ComputeHash(bytText);
			//StringBuilder stringBuilder =  new StringBuilder();
			//foreach(byte b in hash){
			//	stringBuilder.Append(b.ToString("x2"));
			//}
			//return stringBuilder.ToString();
		}
        /// <summary>
        /// Returns SHA256 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA256(Object textToHash, Object key)
        {
            byte[] bytText = AsBytes(textToHash);
            byte[] bytKey = AsBytes(key);
            return Convert.ToBase64String(new HMACSHA256(bytKey).ComputeHash(bytText));
            //byte[] hash=  new HMACSHA256(bytKey).ComputeHash(bytText);
            //StringBuilder stringBuilder =  new StringBuilder();
            //foreach(byte b in hash){
            //	stringBuilder.Append(b.ToString("x2"));
            //}
            //return stringBuilder.ToString();
        }
        /// <summary>
        /// Returns SHA384 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA384(Object textToHash, Object key){
			byte[] bytText=AsBytes(textToHash);
			byte[] bytKey=AsBytes(key);
			return Convert.ToBase64String(new HMACSHA384(bytKey).ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA512 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		/// <param name="key">Cryptographic key</param>
		public string HmacSHA512(Object textToHash, Object key){
			byte[] bytText=AsBytes(textToHash);
			byte[] bytKey=AsBytes(key);
			return Convert.ToBase64String(new HMACSHA512(bytKey).ComputeHash(bytText));
		}
        /// <summary>
        /// Returns SHA1 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA1Hex(Object textToHash, Object key)
        {
            byte[] bytText = AsBytes(textToHash);
            byte[] bytKey = AsBytes(key);
            return ByteArrayToHexadecimalString(new HMACSHA1(bytKey).ComputeHash(bytText));
            //byte[] hash=  new HMACSHA1(bytKey).ComputeHash(bytText);
            //StringBuilder stringBuilder =  new StringBuilder();
            //foreach(byte b in hash){
            //	stringBuilder.Append(b.ToString("x2"));
            //}
            //return stringBuilder.ToString();
        }
        /// <summary>
        /// Returns SHA256 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA256Hex(Object textToHash, Object key)
        {
            byte[] bytText = AsBytes(textToHash);
            byte[] bytKey = AsBytes(key);
            return ByteArrayToHexadecimalString(new HMACSHA256(bytKey).ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA384 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA384Hex(Object textToHash, Object key)
        {
            byte[] bytText = AsBytes(textToHash);
            byte[] bytKey = AsBytes(key);
            return ByteArrayToHexadecimalString(new HMACSHA384(bytKey).ComputeHash(bytText));
        }
        /// <summary>
        /// Returns SHA512 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
        /// </summary>
        /// <param name="textToHash">String to be hashed</param>
        /// <param name="key">Cryptographic key</param>
        public string HmacSHA512Hex(Object textToHash, Object key)
        {
            byte[] bytText = AsBytes(textToHash);
            byte[] bytKey = AsBytes(key);
            return ByteArrayToHexadecimalString(new HMACSHA512(bytKey).ComputeHash(bytText));
        }
        #endregion

        #region Nonce
        private byte[] GetRandomBytes32AndTime(){
			byte[] bytRandom=new byte[32];
			new Random().NextBytes(bytRandom);
			byte[] bytTime=Encoding.UTF8.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff"));
			byte[] bytResult=new byte[32+25];
			Buffer.BlockCopy(bytRandom,0,bytResult,0,32);
			Buffer.BlockCopy(bytTime,0,bytResult,32,25);
			System.Diagnostics.Debug.WriteLine(System.Text.Encoding.Default.GetString(bytResult));
			return bytResult;
		}
		/// <summary>
		/// Returns SHA1 Cryptographic Nonce based on random 32 bytes array concatenated with current 25 bytes DateTime as yyyy-MM-dd HH:mm:ss.fffff  http://en.wikipedia.org/wiki/Cryptographic_nonce
		/// </summary>
		public string GetNewNonceSHA1(){
			return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(GetRandomBytes32AndTime()));
		}
		/// <summary>
		/// Returns SHA256 Cryptographic Nonce based on random 32 bytes array concatenated with current 25 bytes DateTime as yyyy-MM-dd HH:mm:ss.fffff  http://en.wikipedia.org/wiki/Cryptographic_nonce
		/// </summary>
		public string GetNewNonceSHA256(){
			return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(GetRandomBytes32AndTime()));
		}
		/// <summary>
		/// Returns SHA384 Cryptographic Nonce based on random 32 bytes array concatenated with current 25 bytes DateTime as yyyy-MM-dd HH:mm:ss.fffff  http://en.wikipedia.org/wiki/Cryptographic_nonce
		/// </summary>
		public string GetNewNonceSHA384(){
			return Convert.ToBase64String(new SHA384CryptoServiceProvider().ComputeHash(GetRandomBytes32AndTime()));
		}
		/// <summary>
		/// Returns SHA512 Cryptographic Nonce based on random 32 bytes array concatenated with current 25 bytes DateTime as yyyy-MM-dd HH:mm:ss.fffff  http://en.wikipedia.org/wiki/Cryptographic_nonce
		/// </summary>
		public string GetNewNonceSHA512(){
			return Convert.ToBase64String(new SHA512CryptoServiceProvider().ComputeHash(GetRandomBytes32AndTime()));
		}
        #endregion

        #region RSA signing and verification
        /// <summary>
        /// returns the .Net RSA private key algorithm from an encrypted .p12 file
        /// </summary>
        /// <param name="keyfile">the fully qualified filename for the encrypted .p12 file</param>
        /// <param name="password">the password for the encrypted file</param>
        /// <returns>the private key as a .Net key algorithm</returns>
        public AsymmetricAlgorithm ImportRSAPrivateKey(string keyfile, string password)
        {
            X509Certificate2 cert = new X509Certificate2(keyfile, password);
            return cert.PrivateKey;
        }
        /// <summary>
        /// returns the .Net RSA public key from an X509 certificate file
        /// </summary>
        /// <param name="keyfile">the fully qualified filename for the certificate file</param>
        /// <returns>the public key in .Net CryptoServiceProvider XML format </returns>
        public PublicKey ImportRSAPublicKey(string keyfile)
        {
            X509Certificate2 cert = new X509Certificate2(keyfile);
            return cert.PublicKey;
        }
        /// <summary>
        /// returns the digital signature for the data presented
        /// </summary>
        /// <param name="message">the data to be signed</param>
        /// <param name="privateKey">the private key as .Net CyrptoServiceProvider XML format</param>
        /// <returns></returns>
        public string CreateRSASignatureMD5(Object message, AsymmetricAlgorithm privateKey)
        {
            byte[] payload = AsBytes(message);
            //The hash to sign.
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(payload);
            RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(privateKey);
            //Set the hash algorithm.
            RSAFormatter.SetHashAlgorithm("MD5");
            //Create a signature for HashValue and return it.
            byte[] signature = RSAFormatter.CreateSignature(hash);
            return Convert.ToBase64String(signature);
        }
        #endregion

        #region Private Methods
        private static string ByteArrayToHexadecimalString(byte[] arrayToConvert){
			System.Text.StringBuilder sb=new StringBuilder();
			for(int i=0;i<arrayToConvert.Length;i++){
				sb.Append(arrayToConvert[i].ToString("x2"));
			}
			return sb.ToString();
		}
		#endregion

	}


}
