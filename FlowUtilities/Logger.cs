using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace FlowUtilities {
	public class Logger {
		string EXCEPTION_DETAILS_DELIMITER=((char)0).ToString();//that is to put a delimiter before the details, so that we can always extract the initial message
		internal string ConnectionString=null;
		internal string LogFilePathName=null;
		internal bool IsFileFallBack=false;
		internal bool FallBackOnWinLog=false;
		internal byte LogLevel=0;
		private const string _D="\t";
		
		#region LastCallLog
		private string _LastCallLog;
		/// <summary>
		/// Immediately query this property after each call or it will be overwritten by the next call
		/// </summary>
		public string LastCallLog{
			get{return _LastCallLog;}
		}
		#endregion
		
		#region Constructor
		//internal Logger(string logFilePathName,byte logLevel){
		//	LogFilePathName=logFilePathName;
		//	if(string.IsNullOrEmpty(LogFilePathName)){LogFilePathName=@"C:\ProgramData\Flow\Log\FlowUtilities.log";}
		//	LogLevel=logLevel;
		//}
		#endregion

		#region Initialize
		/// <summary>
		/// Configures how the logging will be performed
		/// </summary>
		/// <param name="connectionString">If provided, we attempt to write to dbo.integration_error being present and structured as spected by Craig in his email Monday, 27 April 2015 15:19</param>
		/// <param name="logFilePathName">If provided, we attempt to write to that file unless the isFileFallBack is set to true and we failed to log to database.</param>
		/// <param name="isFileFallBack">If set to true, we Log to file only when we failed to log to database</param>
		/// <param name="fallBackOnWinLog">We write to Win Log only when this is set to True and we did not log to either DB or file (either due to configuration or error)</param>
		/// <param name="logLevel">The way to centrally manage how verbose the logging is. Number from 0 to 5 with 0 logging the least and 5 being very verbose. logLevel parameter you set when calling Log method will be matched against this value.</param>
		/// <returns>True if no problem. Check LastCallLog property of the object for more info.</returns>
		public bool Initialize(string connectionString,string logFilePathName,bool isFileFallBack,bool fallBackOnWinLog,byte logLevel){
			_LastCallLog="";
			IsFileFallBack=isFileFallBack;
			FallBackOnWinLog=fallBackOnWinLog;
			LogLevel=logLevel;

			#region Test we can connect to the DB
			try{
				if(!string.IsNullOrEmpty(connectionString)){
					ConnectionString=connectionString;
					using(SqlConnection cn=new SqlConnection(ConnectionString)){
						cn.Open();
						_LastCallLog+="Successfully connected to database"+Environment.NewLine;
						using(SqlCommand cmd=new SqlCommand("select count(*) from dbo.integration_error with(nolock)",cn)){
							object objRecCount=cmd.ExecuteScalar();
							_LastCallLog+="Successfully queried dbo.integration_error, "+objRecCount.ToString()+" records found"+Environment.NewLine;
						}
					}
				}
				else{
					_LastCallLog+="connectionString not provided, skipping related tests."+Environment.NewLine;
				}
			}
			catch(Exception e){
				_LastCallLog="Failed Database tests for Connection string ["+connectionString+"]. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
				return false;
			}
			#endregion

			#region Test we can write to a file
			try{
				if(!string.IsNullOrEmpty(logFilePathName)){
					LogFilePathName=logFilePathName;
					string strError=WriteLineToFileWithExceptionReturned(logFilePathName,"\r\n\r\n--------------------------------INITIALIZING FlowUtilities.Logger CLASS ----------------------------------------------------------");
					if(!string.IsNullOrEmpty(strError)){throw new Exception(strError);}
					LogMessageToFile("error_entity_type\terror_entity_id\terror_code\taction\taction_step\tsystem\tsystem_error_code\tsystem_error_detail\tmessage",0);
				}
				else{
					_LastCallLog+="logFilePathName not provided, skipping related tests."+Environment.NewLine;
				}
			}
			catch(Exception e){
				_LastCallLog="Failed Log File tests for logFilePathName ["+logFilePathName+"]. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
				return false;
			}
			#endregion

			#region Test we can write to Windows Log
			try{
				if(fallBackOnWinLog){
					System.Diagnostics.EventLog.WriteEntry("FlowUtilities","--------------------------------INITIALIZING LOGGER CLASS ----------------------------------------------------------",System.Diagnostics.EventLogEntryType.Information);
					_LastCallLog+="Successfully wrote to Windows Event Log"+Environment.NewLine;
				}
				else{
					_LastCallLog+="fallBackOnWinLog not requested, skipping related tests."+Environment.NewLine;
				}
			}
			catch(Exception e){
				_LastCallLog="Failed writing to Windows Event Log. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
				return false;
			}
			#endregion
			return true;
		}
		#endregion

		#region Log
		/// <summary>
		/// Logs record as configured in Initialize method
		/// </summary>
		/// <param name="logLevel">Level of verbosity of this message. At run time it should be equal or less than logLevel configured when calling Initialize method. That way the Initialize method becomes the central place to control verbosity of logging without change to this call</param>
		/// <param name="error_entity_type">identifier like “customer” or “order” etc</param>
		/// <param name="error_entity_id">corresponding customer or order id that you are using in the integration</param>
		/// <param name="error_code">code you have defined (corresponding to a constant in Common.inc) in your integration for customer or order processing</param>
		/// <param name="action">action name</param>
		/// <param name="action_step">current map or script name</param>
		/// <param name="system">source or destination system you are interacting with (“magento”, “rms”, “mailchimp” etc)</param>
		/// <param name="system_error_code">error code (if any) returned by the target system</param>
		/// <param name="system_error_detail">all the detail you have (e.g.: error messages, exception traces etc)</param>
		/// <param name="message">data you want shown to a user about this error</param>
		/// <returns>True if no logging failure occured. Check LastCallLog property of the object for more info if problem occurs</returns>
		public bool Log(byte logLevel,string error_entity_type,string error_entity_id,int error_code,string action,string action_step,string system,string system_error_code,string system_error_detail,string message){
			#region Log to DB
			bool blnFailedPrimaryMedia=false;
			bool blnFailedSecondaryMedia=false;
			if(!string.IsNullOrEmpty(ConnectionString)){
				try{
					using(SqlConnection cn=new SqlConnection(ConnectionString)){
						cn.Open();
						using(SqlCommand cmd=new SqlCommand("insert dbo.integration_error([error_entity_type],[error_entity_id],[error_code],[action],[action_step],[system],[system_error_code],[system_error_detail],[message],[inserted]) values (@error_entity_type,@error_entity_id,@error_code,@action,@action_step,@system,@system_error_code,@system_error_detail,@message,@inserted)",cn)){
							SqlParameter p;
							p=cmd.Parameters.Add("@error_entity_type",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(error_entity_type,100);
							p=cmd.Parameters.Add("@error_entity_id",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(error_entity_id,100);
							p=cmd.Parameters.Add("@error_code",SqlDbType.Int);p.Value=error_code;
							p=cmd.Parameters.Add("@action",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(action,100);
							p=cmd.Parameters.Add("@action_step",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(action_step,100);
							p=cmd.Parameters.Add("@system",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(system,100);
							p=cmd.Parameters.Add("@system_error_code",SqlDbType.VarChar,100);p.Value=LeftAndTrimForDB(system_error_code,100);
							p=cmd.Parameters.Add("@system_error_detail",SqlDbType.NVarChar,-1);p.Value=TrimSafeForDB(system_error_detail);
							p=cmd.Parameters.Add("@message",SqlDbType.NVarChar,-1);p.Value=TrimSafeForDB(message);
							p=cmd.Parameters.Add("@inserted",SqlDbType.DateTime);p.Value=DateTime.Now;
							cmd.ExecuteNonQuery();
						}
					}
				}
				catch(Exception e){
					_LastCallLog="Failed to insert into dbo.integration_error. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
					blnFailedPrimaryMedia=true;
				}
			}
			#endregion

			string strFieldsMerged=TrimSafeForDB(error_entity_type)+_D+TrimSafeForDB(error_entity_id)+_D+error_code.ToString()+_D+TrimSafeForDB(action)+_D+TrimSafeForDB(action_step)+_D+TrimSafeForDB(system)
			+_D+TrimSafeForDB(system_error_code)+_D+TrimSafeForDB(system_error_detail)+_D+TrimSafeForDB(message);
			
			#region Log to file
			if(!string.IsNullOrEmpty(LogFilePathName) && (!IsFileFallBack || blnFailedPrimaryMedia)){
				try{
					LogMessageToFile(strFieldsMerged,logLevel);
				}
				catch(Exception e){
					_LastCallLog="Failed to write into LogFilePathName. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
					blnFailedSecondaryMedia=true;
					if(!IsFileFallBack){blnFailedPrimaryMedia=true;}
				}
			}
			#endregion

			#region Log to Windows - that is when everything failed
			if(FallBackOnWinLog && (blnFailedPrimaryMedia || string.IsNullOrEmpty(ConnectionString)) && (blnFailedSecondaryMedia || string.IsNullOrEmpty(LogFilePathName))){
				System.Diagnostics.EventLogEntryType EntryType=System.Diagnostics.EventLogEntryType.Information;
				if(logLevel==0){EntryType=System.Diagnostics.EventLogEntryType.Error;}
				else if(logLevel<4){EntryType=System.Diagnostics.EventLogEntryType.Warning;}
				try{
					System.Diagnostics.EventLog.WriteEntry("FlowUtilities",strFieldsMerged,EntryType);
				}
				catch(Exception e){
					_LastCallLog="Failed to write into Windows Log. ERROR: "+e.Message+e.StackTrace+System.Environment.NewLine+_LastCallLog;
				}
			}
			#endregion

			return !blnFailedPrimaryMedia &&  !blnFailedSecondaryMedia;
		} 
		#endregion

		internal void LogMessageToFile(string message,byte messageLogLevel){
			#region Immediately check if we are logging before collecting any info
			if(LogLevel<messageLogLevel){return;}//Our App log level is not high enough to warrant logging that message
			#endregion
			
			string strAction="Start";
			string strMessageToThrowToCaller="";
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			try{
				if(message!=null){//otherwise we want just an empty line in our log.
					sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
					#region Get the calling method
					System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
					System.Diagnostics.StackFrame stackFrame;
					System.Reflection.MethodBase methodBase=null;
					for(int i=1;i<10;i++){//The trace may be full of our internal stuff which we don't want
						stackFrame = stackTrace.GetFrame(i);
						methodBase = stackFrame.GetMethod();
						if(methodBase.DeclaringType.Name!="Logger")break; 
					}
					#endregion
					#region Create the message's meta tag. It is needed for both - Log and Alert
					strAction="MetaTag";
					sb.Append(_D);
					sb.Append(System.Threading.Thread.CurrentThread.GetHashCode().ToString());
					sb.Append(" ");sb.Append(System.Threading.Thread.CurrentThread.Name);
					sb.Append(_D);sb.Append(messageLogLevel);sb.Append("/");sb.Append(LogLevel);
					sb.Append(_D);sb.Append(System.Environment.MachineName);
					sb.Append(_D);sb.Append(System.Environment.UserName);
					sb.Append(_D);sb.Append(methodBase.DeclaringType.Name);sb.Append(".");sb.Append(methodBase.Name);sb.Append(": ");
					sb.Append(message);
					methodBase=null;stackFrame=null;stackTrace=null;
					#endregion
				}
				strMessageToThrowToCaller=WriteLineToFileWithExceptionReturned(LogFilePathName,sb.ToString());
			}
			catch(Exception e){//here we catch any unexpected exception
				sb=new System.Text.StringBuilder(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
				sb.Append("\tUNEXPECTED ERROR.\r\nAction=[");
				sb.Append(strAction);sb.Append("]\r\nError=[");sb.Append(e.Message);sb.Append("], User=");sb.Append(System.Environment.UserName);
				sb.Append("\rLocationce=[");sb.Append(e.Source);sb.Append("]\r\nStack Trace=[");sb.Append(e.StackTrace);sb.Append("]");
				throw new Exception(sb.ToString());
			}
			if(!string.IsNullOrEmpty(strMessageToThrowToCaller))throw new Exception(strMessageToThrowToCaller);
		}

		internal void Debug(string message){
			LogMessageToFile(message,5);
		}
		internal void Warn(string message){
			LogMessageToFile(message,2);
		}
		internal void Fatal(string message){
			LogMessageToFile(message,0);
		}
		internal void Fatal(string action,Exception e,bool throwBack){
            string strAction=(string.IsNullOrEmpty(action))?"":strAction="Action:["+action+"]\r\n";
			
			#region Get Exception Type
			string strType=e.GetType().FullName;
			if(e.Message.StartsWith(strType)){
				strType="";
			}
			#endregion

			#region Use LogMessage
			//Temporarily place inner exceptions first and see what happens. Otherwise all System.Web.HttpUnhandledException look the same and don't trigger emails string strMsg = strType+e.Message+EXCEPTION_DETAILS_DELIMITER+Environment.NewLine+GetInfoSpecificToExceptionType(e)+GetInnerExceptions(e)+strAction
			string strMsg=GetInnerExceptions(e)+strType+": "+e.Message+EXCEPTION_DETAILS_DELIMITER+Environment.NewLine+strAction
			+"Stack Trace:{"+e.StackTrace+"}";
			Fatal(strMsg.Replace(EXCEPTION_DETAILS_DELIMITER,""));//We do not need the delimiter logged, but want it be passed on
			#endregion
			
			if(throwBack){throw e;}
		}


		#region WriteLineToFileWithExceptionReturned
		/// <summary>
		/// Writes to file. 
		/// </summary>
		/// <param name="fileName">File path name</param>
		/// <returns>Error message</returns>
		[System.Diagnostics.DebuggerStepThrough] 
		string WriteLineToFileWithExceptionReturned(string fileName, string text){
			System.IO.StreamWriter sw=null;
			try{
				#region give it many attempts to write. It may be deceiving to write out of sequence, so we limit ourselves to 100ms
				for(int i=1;i<=50;i++){//give it a preliminaty series of tries with no error
					try{
						sw = new System.IO.StreamWriter(fileName,true);
						if(i==1){
							sw.WriteLine(text);
						}
						else{
							sw.WriteLine(text+"  WA"+i.ToString());
						}
						return null;
					}
					catch{System.Threading.Thread.Sleep(20);}
					finally{if(sw!=null)sw.Dispose();}
				}
				#endregion
				lock(typeof(System.IO.StreamWriter)){
					sw = new System.IO.StreamWriter(fileName,true);
					sw.WriteLine(text);
					return null;
				}
			}
			catch(Exception e){return "Error writing to ["+fileName+"]: "+e.Message;}
			finally{if(sw!=null)sw.Dispose();}
		}
		#endregion


		#region GetInnerExceptions
		private static string GetInnerExceptions(Exception ex){
			string strLeadingTabs="";
			System.Text.StringBuilder sb=new System.Text.StringBuilder();
			while(ex.InnerException != null) {
				strLeadingTabs+=_D;
				sb.Append(strLeadingTabs);
				sb.AppendLine(ex.InnerException.Message);
				sb.Append(strLeadingTabs);
				sb.Append("Stack:{");
				if(!string.IsNullOrEmpty(ex.InnerException.StackTrace)){
					sb.AppendLine();
					sb.Append(strLeadingTabs);
					sb.AppendLine(ex.InnerException.StackTrace).Replace(Environment.NewLine,Environment.NewLine+strLeadingTabs);
					sb.Append(strLeadingTabs);
				}
				sb.AppendLine("}");
				ex = ex.InnerException;
			}
			if(sb.Length > 0) { 
				//Comment out while this is the first what is shown to user sb.Insert(0,"INNER EXCEPTIONS:\r\n");
				sb.Insert(0,"INNER EXCEPTIONS START\r\n");
				sb.AppendLine("END OF INNER EXCEPTIONS.");
			}
			return sb.ToString();
		}
		#endregion

		object LeftAndTrimForDB(string value, int maxLength){
			if(value==null)return DBNull.Value;
			value=value.Trim();
			return (value.Length <= maxLength ? value : value.Substring(0, maxLength));
		}
		object TrimSafeForDB(string value){
			if(value==null)return DBNull.Value;
			return value.Trim();
		}
		
	}

	public enum enuErrorNumbers{
		ServerConnectionOrAuthentication=1,
		MessagesListing=2,
		GettingMessageFromServer=3,
		ParsingMessage=4,
		Filtering=5,
		Dumping=6,
		DeletingMessage=7,
	}


}
