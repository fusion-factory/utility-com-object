using System;

namespace FlowUtilities {
	/// <summary>
	/// Web-related methods
	/// </summary>
	public class Web{
		
		#region URI escape - unescape
		/// <summary>
		/// Converts a string to its escaped representation. Will use %20 for spaces
		/// </summary>
		/// <param name="uriToEncode">URI string to escape</param>
		public string UriEscapeDataString(string uriToEncode){
			return Uri.EscapeDataString(uriToEncode);
		}
		/// <summary>
		/// Converts a string to its unescaped representation. Will expect spaces encoded as %20
		/// </summary>
		/// <param name="uriToDecode">URI string to unescape</param>
		public string UriUnescapeDataString(string uriToDecode){
			return Uri.UnescapeDataString(uriToDecode);
		}
		#endregion

		#region URL encode-unencode
		/// <summary>
		/// Encodes a URL string. Will encode spaces as +
		/// </summary>
		/// <param name="urlToEncode">URL to be encoded</param>
		public string UrlEncode(string urlToEncode){
			return System.Web.HttpUtility.UrlEncode(urlToEncode);
		}
		/// <summary>
		/// Converts a string that has been encoded for transmission in a URL into a decoded string. Will replace + with spaces
		/// </summary>
		/// <param name="urlToDecode">URL to be decoded</param>
		/// <returns></returns>
		public string UrlDecode(string urlToDecode){
			return System.Web.HttpUtility.UrlDecode(urlToDecode);
		}
		#endregion


		/*
		#region System - Temporary thing to test Flow bug
		//TODO: DELETE WHEN CRAIG IS DONE WITH PROOF OF CONCEPT
		string _System;
		public string System1{
			set{_System=value;}
			get{return _System;}
		}
		#endregion
		*/



	}
}
