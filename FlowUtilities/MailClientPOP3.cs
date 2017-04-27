using System;
using System.Text.RegularExpressions;
using System.Collections;
using ActiveUp.Net.Mail;

namespace FlowUtilities {
	public class MailClientPOP3 {
		Logger _Log;
		string LogFilePathName;
		byte LogLevel=0;

		public MailClientPOP3(string logFilePathName,byte logLevel){
			LogFilePathName=logFilePathName;
			LogLevel=logLevel;
//			_Log=new Logger(LogFilePathName,LogLevel);
		}
		
		public int DumpMailboxToDirectory_SSL(out int errorNumber,out string errorDescription,string host,int port,string username,string password,bool ssl,bool deleteDumpedMessage
		,bool dumpHeaders,bool dumpBody,bool dumpMatchingAttachment,string headerRegEx,string bodyRegEx,string attachmentRegEx,string dumpPathName){
			string strResponse=null;
			int intMessagesDumpedCount=0;
			errorNumber=0;
			errorDescription=null;
			Pop3Client Client=new Pop3Client();
			//string strAction;
			try{
				#region Establish connection and authenticate to the server
				errorDescription="Attempting to connect and Authenticate to "+host+" on "+port.ToString()+" as "+username;
				_Log.Debug(errorDescription);
				errorNumber=(int)enuErrorNumbers.ServerConnectionOrAuthentication;
				if(ssl){strResponse=Client.ConnectSsl(host,port,username,password);}
				else{strResponse=Client.Connect(host,port,username,password);}
				if(strResponse.StartsWith("-ERR",StringComparison.OrdinalIgnoreCase)){
					errorDescription="Connecting or Authenticating to the server: "+strResponse;
					return 0;
				}
				_Log.Debug("Successfully Authenticated to "+host+" on "+port.ToString()+" as "+username);
				#endregion

				#region Get list of the messages there
				errorDescription="Attempting to get messages count from "+host+" as "+username;
				_Log.Debug(errorDescription);
				errorNumber=(int)enuErrorNumbers.MessagesListing;
				int intMessagesCount=Client.MessageCount;
				if(intMessagesCount==0){
					_Log.Debug("No message on "+host+" for "+username);
					errorDescription=null;
					errorNumber=0;
					return 0;
				}
				#endregion

				#region Go through messages and download (and optionally delete) them
				
				for(int i=1;i<=intMessagesCount;i++){
					errorDescription="Attempting to get message "+i.ToString()+" from "+host+" as "+username;
					_Log.Debug(errorDescription);
					errorNumber=(int)enuErrorNumbers.GettingMessageFromServer;
					byte[] MessageBytes=Client.RetrieveMessage(i,false);
					if(MessageBytes.Length==0){
						_Log.Debug("Message "+i.ToString()+" does not exist on "+host+":"+username);
						continue;
					}
					else{
						//Parser Parser=new ActiveUp.Net.Mail.Parser();
						errorNumber=(int)enuErrorNumbers.ParsingMessage;
						errorDescription="Attempting to parse message "+i.ToString()+" from "+host+" as "+username;
						Message Msg=Parser.ParseMessage(MessageBytes);
							
						errorNumber=(int)enuErrorNumbers.Filtering;
						errorDescription="Attempting to apply filters to message "+i.ToString()+" from "+host+" as "+username;
						#region See if we have a match to headers RegEx
						Regex RE=null;
						bool blnDumpMessage=true;
						if(!string.IsNullOrEmpty(headerRegEx)){
							blnDumpMessage=false;
							RE=new Regex(headerRegEx);//Subject: Automatic reply: Brian, Welcome to hell
							for(int ii=0;ii<Msg.HeaderFieldNames.Count;ii++){
								if(RE.IsMatch(Msg.HeaderFieldNames[ii]+": "+Msg.HeaderFields[ii])){
									blnDumpMessage=true;
									break;//We will be recording all headers anyway
								}
							}
							if(!blnDumpMessage)continue;//There was filter, and we failed it
						}
						#endregion

						#region See if we have a match to the body
						if(!string.IsNullOrEmpty(bodyRegEx)){
							blnDumpMessage=false;
							RE=new Regex(bodyRegEx);
							if(!RE.IsMatch(Msg.BodyText.Text))continue;//There was filter, and we failed it
							blnDumpMessage=true;
						}
						#endregion
							
						#region See if any attachment name matches
						System.Collections.Generic.List<int> lstAttachmentsToDump=new System.Collections.Generic.List<int>();
						if(!string.IsNullOrEmpty(attachmentRegEx)){
							blnDumpMessage=false;
							RE=new Regex(attachmentRegEx);
							for(int ii=0;ii<Msg.Attachments.Count;ii++){
								if(RE.IsMatch(Msg.Attachments[ii].Filename)){
									blnDumpMessage=true;
									lstAttachmentsToDump.Add(ii);
								}
							}
							if(!blnDumpMessage)continue;//There was filter, and we failed it
						}
						#endregion

						#region Dump what was requested if the message passed the filters
						if(blnDumpMessage){
							errorNumber=(int)enuErrorNumbers.Dumping;
							errorDescription="Preparing to dump message "+i.ToString()+" from "+host+" as "+username;
							_Log.Debug(errorDescription);
							string strMessageId=Msg.MessageId;
							if(string.IsNullOrEmpty(strMessageId)){strMessageId=Guid.NewGuid().ToString();}
							string strFileNamePrimer=dumpPathName.TrimEnd(new char[]{'\\'})+"\\"+strMessageId+"_";
								
							#region Dump Headers
							if(dumpHeaders){
								errorDescription="About to dump headers "+i.ToString()+" from "+host+" as "+username;
								_Log.Debug(errorDescription);
								lock(typeof(System.IO.StreamWriter)){
									using(System.IO.StreamWriter sw = new System.IO.StreamWriter(strFileNamePrimer+"HEAD.txt",true)){
										for(int ii=0;ii<Msg.HeaderFieldNames.Count;ii++){
											sw.WriteLine(Msg.HeaderFieldNames[ii]+": "+Msg.HeaderFields[ii]);
										}
									}
								}
							}
							#endregion
								
							#region Dump Body
							if(dumpBody){
								errorDescription="About to dump body "+i.ToString()+" from "+host+" as "+username;
								_Log.Debug(errorDescription);
								lock(typeof(System.IO.StreamWriter)){
									using(System.IO.StreamWriter sw = new System.IO.StreamWriter(strFileNamePrimer+"BODY.txt",true)){
										sw.Write(Msg.BodyText.Text);
									}
								}
							}
							#endregion
								
							#region Dump Attachments
							if(dumpMatchingAttachment){
								errorDescription="About to dump attachments from message "+i.ToString()+" from "+host+" as "+username;
								for(int ii=0;ii<Msg.Attachments.Count;ii++){
									if(string.IsNullOrEmpty(attachmentRegEx) || lstAttachmentsToDump.Contains(ii)){
										errorDescription="About to dump attachment ["+Msg.Attachments[ii].Filename+"] from message "+i.ToString()+" from "+host+" as "+username;
										_Log.Debug(errorDescription);
										Msg.Attachments[ii].StoreToFile(strFileNamePrimer+Msg.Attachments[ii].Filename);
									}
								}
								intMessagesDumpedCount++;//If someone configured to get messages but no any part of message to be dumped, we still count that message.
							}
							#endregion
						}
						#endregion

						if(blnDumpMessage && deleteDumpedMessage){
							errorNumber=(int)enuErrorNumbers.DeletingMessage;
							errorDescription="Attempting to delete message "+i.ToString()+" from "+host+" as "+username;
							_Log.Debug(errorDescription);
							Client.DeleteMessage(i);
						}
					}
				}
				#endregion



			}
			catch(Exception e){
				_Log.Fatal(errorDescription,e,false);
				errorDescription=errorDescription+". ERROR: "+e.Message;
				return 0;
			}
			finally{
				if(Client!=null){
					if(Client.IsConnected){
						try{
							Client.Disconnect();
						}
						catch(Exception e){
							_Log.Fatal("Disconnecting from the "+host,e,false);
						}
					}
				}
			}
			return 0;
		}
	}
}
