using System;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

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

        #region GenerateInclude
        private void GenerateEnumDefinitions(StreamWriter sw, Type t)
        {
            string line = "";
            foreach (Object c in Enum.GetValues(t))
            {
                line = "const " + t.ToString() + "_" + c.ToString() + " = " + ((int)c).ToString() + ";\r\n";
                sw.Write(line.Replace('.', '_'));
            }
            sw.Write("\r\n");
        }
        /// <summary>
        /// generates FlowUtilities.inc file
        /// </summary>
        /// <param name="folderPath">optional - location to place the generated file</param>
        public void GenerateInclude(string folderPath)
        {
            if (folderPath == null || folderPath == "")
                folderPath = "C:\\ProgramData\\Flow\\Script";
            string fileName = folderPath + "\\" + "FlowUtilities.inc";
            string header = "{$IfNDef FlowUtilties_Included}\r\n{$Define FlowUtilties_Included}\r\n\r\n{ Copyright Fusion Factory Pty Ltd 2017}\r\n\r\n";
            string footer = "{$EndIf}\r\n";
            StreamWriter sw = System.IO.File.CreateText(fileName);
            sw.Write(header);

            GenerateEnumDefinitions(sw, typeof(CipherMode));
            GenerateEnumDefinitions(sw, typeof(PaddingMode));

            sw.Write(footer);
            sw.Close();

        }
        #endregion

    }
}
