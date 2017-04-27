/*using System;
using System.Text;
using System.Net.Security;
using System.Net.Sockets;

namespace FlowUtilities {
	public class MailClient_POP3 {
		string LogDirectory;
		byte LogLevel;

		public int DumpMailboxToDirectory_SSL(string server,int port,string username,string password,string dumpPathName){
			#region Declarations and initializations
			byte[] buffer = new byte[2048];
			TcpClient Client=new TcpClient();
			Client.Connect(server,port);
			SslStream SslStream=new SslStream(Client.GetStream());
			SslStream.ReadTimeout=10000;
			#endregion
			
			#region Authenticate
			SslStream.AuthenticateAsClient(server);
			// Read the stream to make sure we are connected
			int intBytesCount=SslStream.Read(buffer, 0, buffer.Length);
			string strResponse=Encoding.ASCII.GetString(buffer, 0, intBytesCount);
			//TODO: Log
			strResponse=GetResponse_SSL(SslStream,"USER "+username+"\r\n");
			strResponse=GetResponse_SSL(SslStream,"PASS "+password+"\r\n");//-ERR [AUTH] Web login required: https://support.google.com/mail/bin/answer.py?answer=78754	https://www.google.com/settings/security/lesssecureapps
			if(strResponse.StartsWith("-ERR",StringComparison.OrdinalIgnoreCase))throw new Exception("ERROR Authenticating to the server: "+strResponse);
			//TODO: Log
			#endregion

			#region Keep dumping the messages in the 
			while(true){
				SslStream.Write(Encoding.ASCII.GetBytes("LIST\r\n"));
				strResponse=GetMessage_SSL(SslStream);
				if(!strResponse.StartsWith("+OK",StringComparison.OrdinalIgnoreCase))throw new Exception("Failed to LIST messages. Server response: "+strResponse);
				if(!strResponse.StartsWith("+OK 0",StringComparison.OrdinalIgnoreCase)){
					//LOG
					break;
				}
				SslStream.Write(Encoding.ASCII.GetBytes("RETR 1\r\n"));
				string msg=GetMessage_SSL(SslStream);//That's the entire message with headers and attachments


			}
			#endregion






			return 0;
		}
		
		private string GetMessage_SSL(System.Net.Security.SslStream stream){
			byte[] buffer = new byte[2048];
			StringBuilder sbMessage = new StringBuilder();
			StringBuilder sbBuffer=new StringBuilder();
			int bytes = -1;
			while(bytes!=0){//Actually it is never 0 as the server never returns 0 bytes, at least gmail
				try{
					bytes = stream.Read(buffer, 0, buffer.Length);//That hangs when we read past the end of the message, though 16 bytes are returned
				}
				catch(Exception e){
					throw new Exception("Either genuine timeout on the server or we managed to read past the end of the message. ERROR:["+e.Message+"]. The buffer so far:["+sbMessage+"]");
				}
				Decoder decoder = Encoding.UTF8.GetDecoder();
				char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
				decoder.GetChars(buffer, 0, bytes, chars, 0);
				sbBuffer.Clear();
				sbBuffer.Append(chars);
				sbMessage.Append(sbBuffer);
				if(sbBuffer.ToString().Contains("\r\n.\r\n")){
					break;
				}
			}
			return sbMessage.ToString();
		}

		private string GetResponse_SSL(SslStream stream,string request){
			int intBytesCount;
			byte[] buffer = new byte[2048];
			stream.Write(Encoding.ASCII.GetBytes(request));
			intBytesCount=stream.Read(buffer, 0, buffer.Length);
			return Encoding.ASCII.GetString(buffer, 0, intBytesCount);
		}

	}
}
*/