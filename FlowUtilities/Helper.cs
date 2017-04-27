using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Globalization;

namespace FlowUtilities {
	/// <summary>
	/// Helper Methods not falling into any category
	/// </summary>
	public class Helper {
		[DllImport("rpcrt4.dll",SetLastError = true)]
		static extern int UuidCreateSequential(out Guid guid);

		#region TestComRegistration
		/// <summary>
		/// Used by installer to test registration with COM
		/// </summary>
		/// <returns>String confirming that it was called successfully</returns>
		public string TestComRegistration(){
			return "Successfully called via COM";
		}
		#endregion

		#region GetNewSequentialSqlGuidAsString
		/// <summary>
		/// Returns a new (temporarily) sequential GUID tweaked for SQL Server understanding of being sequential. From my observation it is not guaranteed to be sequential for any given data table, it is just sequential for the system at a given time.
		/// </summary>
		public string GetNewSequentialSqlGuidAsString() {
			Guid guid;
			UuidCreateSequential(out guid);
			var s = guid.ToByteArray();
			var t = new byte[16];
			t[3] = s[0];
			t[2] = s[1];
			t[1] = s[2];
			t[0] = s[3];
			t[5] = s[4];
			t[4] = s[5];
			t[7] = s[6];
			t[6] = s[7];
			t[8] = s[8];
			t[9] = s[9];
			t[10] = s[10];
			t[11] = s[11];
			t[12] = s[12];
			t[13] = s[13];
			t[14] = s[14];
			t[15] = s[15];
			return new Guid(t).ToString();
		}
		#endregion

		#region RemoveDiacritics
		/// <summary>
		/// Removes accents from the text
		/// </summary>
		/// <param name="text">Text to remove the Diacritics from</param>
		/// <returns>Text with Diacritics removed</returns>
		public string RemoveDiacritics(string text){
		  return string.Concat( 
			  text.Normalize(NormalizationForm.FormD)
			  .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch)!=
											UnicodeCategory.NonSpacingMark)
			).Normalize(NormalizationForm.FormC);
		}
		#endregion

	}
}
