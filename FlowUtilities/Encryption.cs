using System;
using System.Text;
using System.Security.Cryptography;

namespace FlowUtilities {
	/// <summary>
	/// Methods to perform Encryption 
	/// </summary>
	public class Encryption {
		
		#region Regular Hashes with immediate Base64 bit encoding
		/// <summary>
		/// Returns MD5 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string MD5Hash(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA1 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA1Hash(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA256 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA256Hash(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA384 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA384Hash(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(new SHA384CryptoServiceProvider().ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA512 Hash of the string immediately Base64 encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA512Hash(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(new SHA512CryptoServiceProvider().ComputeHash(bytText));
		}
		#endregion
		
		#region Regular Hashes with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// <summary>
		/// Returns MD5 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string MD5HashHexBase64(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(ByteArrayToHexadecimalString(new MD5CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA1 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA1HashHexBase64(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(ByteArrayToHexadecimalString(new SHA1CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA256 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA256HashHexBase64(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(ByteArrayToHexadecimalString(new SHA256CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA384 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA384HashHexBase64(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(ByteArrayToHexadecimalString(new SHA384CryptoServiceProvider().ComputeHash(bytText))));
		}
		/// <summary>
		/// Returns SHA512 Hash with each hashed bit converted to hexadecimal string, then Base64 bit encoded
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		public string SHA512HashHexBase64(string textToHash){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			return Convert.ToBase64String(Encoding.ASCII.GetBytes(ByteArrayToHexadecimalString(new SHA512CryptoServiceProvider().ComputeHash(bytText))));
		}
		#endregion


		#region Keyed hashes
		/// <summary>
		/// Returns MD5 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		/// <param name="key">Cryptographic key</param>
		public string HmacMD5(string textToHash,string key){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			byte[] bytKey=Encoding.ASCII.GetBytes(key);
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
		public string HmacSHA1(string textToHash,string key){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			byte[] bytKey=Encoding.ASCII.GetBytes(key);
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
		public string HmacSHA256(string textToHash,string key){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			byte[] bytKey=Encoding.ASCII.GetBytes(key);
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
		public string HmacSHA384(string textToHash,string key){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			byte[] bytKey=Encoding.ASCII.GetBytes(key);
			return Convert.ToBase64String(new HMACSHA384(bytKey).ComputeHash(bytText));
		}
		/// <summary>
		/// Returns SHA512 HMAC of the string. http://en.wikipedia.org/wiki/Hash-based_message_authentication_code
		/// </summary>
		/// <param name="textToHash">String to be hashed</param>
		/// <param name="key">Cryptographic key</param>
		public string HmacSHA512(string textToHash,string key){
			byte[] bytText=Encoding.ASCII.GetBytes(textToHash);
			byte[] bytKey=Encoding.ASCII.GetBytes(key);
			return Convert.ToBase64String(new HMACSHA512(bytKey).ComputeHash(bytText));
		}
		#endregion
	
		#region Nonce
		private byte[] GetRandomBytes32AndTime(){
			byte[] bytRandom=new byte[32];
			new Random().NextBytes(bytRandom);
			byte[] bytTime=Encoding.ASCII.GetBytes(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffff"));
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
