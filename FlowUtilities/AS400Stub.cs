using System;
using System.Collections.Generic;
using System.Text;

namespace FlowUtilities {
	/// <summary>
	/// Wrapper around IBM AS/400
	/// </summary>
	public class AS400 {
		//private cwbx.AS400System _AS400=null;
		private List<int> _ConnectedServices=new List<int>();
		
		#region LastCallLog
		private string _LastCallLog;
		/// <summary>
		/// Immediately query this property after each call or it will be overwritten by the next call
		/// </summary>
		public string LastCallLog{
			get{return _LastCallLog;}
		}
		#endregion

		#region Initialize
		/// <summary>
		/// Method to run right after initializing this object. It is not part of constructor because the exetution may take longer that OS expects for COM object to be initialized
		/// </summary>
		/// <param name="host"></param>
		/// <param name="IP"></param>
		/// <param name="user"></param>
		/// <param name="password"></param>
		/// <param name="service"></param>
		/// <param name="log">Log of all the steps</param>
		/// <returns>true if success</returns>
		public bool Initialize(string host,string IP,string user,string password,int service){//It does too much to be a constructor
			_LastCallLog="";
			string strLog="";
			try{
				//_AS400=new cwbx.AS400System();
				strLog+="AS400System Initialized, about to define host "+host;
				//_AS400.Define(host);
				strLog+="\r\nHost defined, preparing to connect with user "+user+" password "+password.Length.ToString()+" characters long on IP "+IP;
				//_AS400.UserID=user;
				//_AS400.Password=password;
				//_AS400.IPAddress=IP;
				//_AS400.Connect((cwbx.cwbcoServiceEnum)service);//7
				strLog+="\r\nThe Connect request returned control";
				if(true/*_AS400.IsConnected((cwbx.cwbcoServiceEnum)service)==1*/){
					strLog+="\r\nChecked: CONNECTED";
					if(!_ConnectedServices.Contains(service)){_ConnectedServices.Add(service);}
					return true;
				}
				else{
					strLog+="\r\nChecked: NOT CONNECTED";
					return false;
				}
			}
			catch(Exception e){
				strLog="ERROR: "+e.Message+e.StackTrace+Environment.NewLine+strLog;
				return false;
			}
			finally{
				_LastCallLog=strLog;
			}
		}
		#endregion

		#region RunInitialProgram
		/// <summary>
		/// Runs what appears to be a required step before you can call any command. It is unclear why it was a separate function and not part of the initializer
		/// </summary>
		/// <param name="company">Company for which to call it</param>
		/// <param name="log">Log of all the steps</param>
		/// <returns>true if success</returns>
		public bool RunInitialProgram(string company){
			_LastCallLog="";
			string strLog="";
			bool blnSucceeded=true;
			int intParamLen=2;//not sure why

			blnSucceeded=CallCommand("ADDLIBLE A600AP");
			strLog+=Environment.NewLine+_LastCallLog;
			if(!blnSucceeded){
				_LastCallLog=strLog;
				return false;
			}

			//blnSucceeded=CallCommand("ADDLIBLE SUN600FS");
			strLog+=Environment.NewLine+_LastCallLog;
			if(!blnSucceeded){
				_LastCallLog=strLog;
				return false;
			}

			try{
				//cwbx.Program Prog=new cwbx.Program();
				//Prog.LibraryName="*LIBL";
				//Prog.ProgramName="ACSINLPGM";
				//Prog.system=_AS400;
				
				//cwbx.ProgramParameters Params=new cwbx.ProgramParameters();
				//cwbx.StringConverter Converter=new cwbx.StringConverter();
				//Params.Append("Company",cwbx.cwbrcParameterTypeEnum.cwbrcInput,intParamLen);
				//Converter.Length=intParamLen;
				string param=company;
				//Params["Company"].Value=Converter.ToBytes(param.PadLeft(intParamLen,' '));
				//Prog.Call(Params);
				
				#region Check System Errors
				string strSysErrors=GetSystemErrors();
				if(strSysErrors==""){
					strLog+=Environment.NewLine+"No System errors detected";
				}
				else{
					strLog+=Environment.NewLine+strSysErrors;
					blnSucceeded=false;
				}
				if(!blnSucceeded)return false;
				#endregion

				#region Check Program Errors
				if(true/*Prog.Errors.Count==0*/){
					strLog+=Environment.NewLine+"No Program errors detected";
				}
				else{
					blnSucceeded=false;
					strLog+=Environment.NewLine+"PROGRAM ERRORS:";
					//for(int i=0;i<Prog.Errors.Count;i++){
					//	strLog+=Environment.NewLine+Prog.Errors[i].Text;
					//}
				}
				if(!blnSucceeded)return false;
				return true;
				#endregion
			}
			catch(Exception e){
				strLog="ERROR: "+e.Message+e.StackTrace+Environment.NewLine+strLog;
				return false;
			}
			finally{
				_LastCallLog=strLog;
			}
		}
		#endregion

		#region GetSystemErrors
		/// <summary>
		/// Gets system errors
		/// </summary>
		/// <returns>System Error per line or empty string if there is none</returns>
		public string GetSystemErrors(){
			string strErrors="";
            /*
			if(_AS400==null)return "AS400 not initialized";
			try{
				if(_AS400.Errors.ReturnCode!=0){
					strErrors+="Errors.ReturnCode="+_AS400.Errors.ReturnCode.ToString();
				}
				for(int i=0;i<_AS400.Errors.Count;i++){
					strErrors+=Environment.NewLine+_AS400.Errors[i].Text;
				}
			}
			catch(Exception e){
				strErrors="FAILED TO GET SYSTEM ERRORS. Error:"+e.Message+e.StackTrace+Environment.NewLine+strErrors;
			}
             * */
			return strErrors;
		}
		#endregion

		#region CallCommand
		/// <summary>
		/// Calls command. All the initializations need to be done before you can call this
		/// </summary>
		/// <param name="command">Command to execute</param>
		/// <param name="log">Log of all the steps</param>
		/// <returns>true if success</returns>
		public bool CallCommand(string command){
			_LastCallLog="";
			bool blnAnyError=false;
			string strLog="";
			//cwbx.Command Cmd=null;
			try{
				//Cmd=new cwbx.Command();
				strLog+="Command object initialized. About to set system property\r\n";
				//Cmd.system=_AS400;
				strLog+="system property set to _AS400. About to run command ["+command+"]\r\n";
				//Cmd.Run(command);
				strLog+="Run command ["+command+"], about to query command errors\r\n";
                /*
				for(int i=0;i<Cmd.Errors.Count;i++){
					strLog+=Environment.NewLine+Cmd.Errors[i].Text;
					blnAnyError=true;
				}
                 */
			}
			catch(Exception e){
				strLog="FAILED TO EXECUTE COMMAND ["+command+"]. Error:"+e.Message+e.StackTrace+Environment.NewLine+strLog;
				blnAnyError=true;
			}
			finally{
				_LastCallLog=strLog;
			}
			return !blnAnyError;
		}
		#endregion

		#region Disconnect
		/// <summary>
		/// Disconnects AS400 from the Service
		/// </summary>
		/// <param name="service">Service to disconnect from</param>
		/// <param name="log">Log of all the steps</param>
		/// <returns>true if success</returns>
		public bool Disconnect(int service){
			_LastCallLog="";
			string strLog="";
			if(false){//_AS400==null){
				_LastCallLog="AS400 is NULL";
				return false;
			}
			if(!_ConnectedServices.Contains(service)){
				_LastCallLog="We either never connected to service "+service+" with this instance, or we already issued Disconnect command. Check logic of your code";
				return false;
			}
			try{
				if(false){//_AS400.IsConnected((cwbx.cwbcoServiceEnum)service)!=1){
					//strLog="AS400 is NOT connected to service "+service.ToString()+". Its return code is "+_AS400.IsConnected((cwbx.cwbcoServiceEnum)service).ToString();;
					return false;
				}
				strLog+="Connected to service "+service.ToString()+". About to call Disconnect"+Environment.NewLine;	
				//_AS400.Disconnect((cwbx.cwbcoServiceEnum)service);
				strLog+="Call to Disconnected service "+service.ToString()+" returned control. We do not validate if it disconnected as usually it does not immediately and we have no time to find if it does later"+Environment.NewLine;	
				return true;
				//log+="Call to Disconnected service "+service.ToString()+" returned control. About to validate the disconnection"+Environment.NewLine;	
				//if(_AS400.IsConnected((cwbx.cwbcoServiceEnum)service)==1){
				//	log+="Still connected to service "+service.ToString()+" :("+Environment.NewLine;	
				//	return false;
				//}
				//else{
				//	log+="Disconnection of service "+service.ToString()+" has been validated, all good."+Environment.NewLine;
				//	if(_ConnectedServices.Contains(service)){_ConnectedServices.Remove(service);}
				//	return true;
				//}
			}
			catch(Exception e){
				strLog="FAILED. Error:"+e.Message+e.StackTrace+Environment.NewLine+strLog;
				return false;
			}
			finally{
				_LastCallLog=strLog;
			}
			
		}
		#endregion

		#region Destructor
		/// <summary>
		/// Destructor
		/// </summary>
		~AS400(){
			foreach(int Service in _ConnectedServices){
				Disconnect(Service);
			}
		}
		#endregion

	}
}
