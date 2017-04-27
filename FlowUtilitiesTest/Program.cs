using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace FlowUtilitiesTest{
//Visual Studio Express for Web fails to run unit tests, so have to resort to old methods
    public class Program {
        static void Main(string[] args){
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Test());

/*
			//FlowUtilities.MailClient_POP3 Mail=new FlowUtilities.MailClient_POP3();
			//Mail.DumpMailboxToDirectory_SSL("pop.gmail.com",995,"eventchainsystems@gmail.com","EvChSy2008",@"E:\Temp");
			FlowUtilities.MailClientPOP3 Mail=new FlowUtilities.MailClientPOP3(@"E:\Work\Projects\11-FlowUtilities\Log.log",5);
			int intErr;
			string strErr;
			Mail.DumpMailboxToDirectory_SSL(out intErr,out strErr,"pop.gmail.com",995,"eventchainsystems@gmail.com","EvChSy2008",ssl:true,deleteDumpedMessage:true
			,dumpHeaders:true,dumpBody:true,dumpMatchingAttachment:true,headerRegEx:".",bodyRegEx:".",attachmentRegEx:".",dumpPathName:@"E:\Temp");

			FlowUtilities.Web Web=new FlowUtilities.Web();
			string strTestUrl="https://www.flow.net.nz/login.aspx?# space !~@$%%^&*';:?<>username=XXX&password=yyy";
			string strURI=Web.UriEscapeDataString(strTestUrl);
			Debug.WriteLine(strURI);
			Debug.WriteLine(Web.UriUnescapeDataString(strURI));
			string strURL=Web.UrlEncode(strTestUrl);
			Debug.WriteLine(strURL);
			Debug.WriteLine(Web.UrlDecode(strURL));
			
			
			FlowUtilities.Helper h=new FlowUtilities.Helper();
			Debug.Write(h.GetNewSequentialSqlGuidAsString()+"\r\n");

			FlowUtilities.Encryption E=new FlowUtilities.Encryption();
			Debug.WriteLine(E.GetNewNonceSHA1());
			Debug.WriteLine("================================================");
			Debug.WriteLine(E.GetNewNonceSHA256());
			Debug.WriteLine("================================================");
			Debug.WriteLine(E.GetNewNonceSHA384());
			Debug.WriteLine("================================================");
			Debug.WriteLine(E.GetNewNonceSHA512());
			Debug.WriteLine("================================================");

			Debug.WriteLine(E.MD5Hash(""));
			Debug.WriteLine(E.HmacMD5("",""));
			Debug.WriteLine(E.SHA1Hash(""));
			Debug.WriteLine(E.HmacSHA1("",""));
			Debug.WriteLine(E.SHA256Hash(""));
			Debug.WriteLine(E.HmacSHA256("",""));

			Debug.WriteLine(E.MD5HashHexBase64("9eT7H[OGmllf32014-12-04"));
			Debug.WriteLine(E.HmacMD5("The quick brown fox jumps over the lazy dog","key"));
			Debug.WriteLine(E.SHA1Hash("The quick brown fox jumps over the lazy dog"));
			Debug.WriteLine(E.HmacSHA1("The quick brown fox jumps over the lazy dog","key"));
			Debug.WriteLine(E.SHA256Hash("The quick brown fox jumps over the lazy dog"));
			Debug.WriteLine(E.HmacSHA256("The quick brown fox jumps over the lazy dog","key"));

			Debug.WriteLine(E.SHA384Hash("AAAAAAAAAA"));
			Debug.WriteLine(E.HmacSHA384("AAAAAAAAAA","A"));
			Debug.WriteLine(E.SHA512Hash("AAAAAAAAAA"));
			Debug.WriteLine(E.HmacSHA512("AAAAAAAAAA","A"));
*/		}

		static string ToHexString(string text){
			char[] chars = text.ToCharArray();
			StringBuilder stringBuilder =  new StringBuilder();
			foreach(char c in chars){
				stringBuilder.Append(((Int16)c).ToString("x"));
			}
			return stringBuilder.ToString();
		}
	}
}
