using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.Windows.Forms;
using System.Diagnostics;

namespace FlowUtilitiesCustomActionsLibrary {
	public class CustomActions {
		
		#region RegisterDLL
		[CustomAction]
		public static ActionResult RegisterDLL(Session session) {
            session.Log("Begin RegisterDLL &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
			
			#region Get the Install Folder used by the Installer
			session.Log("Get PathName of DLL to register");
			string strInstallFolder=GetWixProperty("INSTALLFOLDER",session,"RegisterDLL",false);
			if(string.IsNullOrEmpty(strInstallFolder))return ActionResult.Failure;
			#endregion

			#region Check the DLL is actually there
			session.Log("Check the DLL is actually there");
			string strDllPathName=strInstallFolder+"FlowUtilities.dll";
			if(!System.IO.File.Exists(strDllPathName)){
				session.Log("File "+strDllPathName+" does not exist");
				MessageBox.Show("DLL File "+strDllPathName+" does not exist","File not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			session.Log("Successfully found DLL at "+strDllPathName);
			#endregion

			#region Find appropriate RegAsm file
			session.Log("Find appropriate RegAsm file");
			List<string> RegAsmPathNames=new List<string>();
			RegAsmPathNames.Add(Environment.GetFolderPath(Environment.SpecialFolder.Windows)+@"\Microsoft.NET\Framework\v4.0.30319\regasm.exe");
			string strOptionsTried="";
			//Here we will be adding other possible directories of which we currently are not aware
			string strRegAsmFile=null;
			foreach(string strFile in RegAsmPathNames){
				strOptionsTried+=strFile+Environment.NewLine;
				if(System.IO.File.Exists(strFile)){
					strRegAsmFile=strFile;
					break;
				}
			}
			if(strRegAsmFile==null){
				session.Log("Could not find appropriate .NET framework to register "+strDllPathName+". Options tried: "+strOptionsTried);
				MessageBox.Show("Could not find appropriate .NET framework to register "+strDllPathName,"File not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			session.Log("Successfully found RegAsm at "+strRegAsmFile);
			#endregion

			#region #Register the DLL
			session.Log("Register the DLL using "+strRegAsmFile);
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName=strRegAsmFile;
			startInfo.Arguments="/codebase \""+strDllPathName+"\"";
			startInfo.WindowStyle=ProcessWindowStyle.Hidden;
			startInfo.UseShellExecute=false;
			Process p=null;
			try{
				p= new Process();
				p.StartInfo = startInfo;
				p.Start();
				p.WaitForExit();
				session.Log("Successfully registered "+strDllPathName+". ExitCode="+p.ExitCode.ToString());
			}
			catch(Exception e){
				session.Log("Failed to execute ["+startInfo.FileName+" "+startInfo.Arguments+"]. ExitCode="+p.ExitCode.ToString()+", Error: "+e.Message);
				MessageBox.Show("Failed to execute ["+startInfo.FileName+" "+startInfo.Arguments+"], Error: "+e.Message,"Failed to register",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			#endregion

			#region Test-run the DLL
			Type typComTest=null;
			object objComTest=null;
			try{
				typComTest=Type.GetTypeFromProgID("FlowUtilities.Helper");
				session.Log("Successfully initialized FlowUtilities.Helper.");
			}
			catch(Exception e){
				session.Log("Failed to initialize FlowUtilities.Helper. ERROR: "+e.Message);
				MessageBox.Show("Failed to initialize FlowUtilities.Helper. ERROR: "+e.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			try{
				objComTest=Activator.CreateInstance(typComTest);
				session.Log("Successfully created instance of FlowUtilities.Helper.");
			}
			catch(Exception e){
				session.Log("Failed to create instance of FlowUtilities.Helper. ERROR: "+e.Message);
				MessageBox.Show("Failed to create instance of FlowUtilities.Helper. ERROR: "+e.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			try{
				object oResult=typComTest.InvokeMember("TestComRegistration",System.Reflection.BindingFlags.InvokeMethod,null,objComTest,null);
				if(oResult==null){
					session.Log("Failed to get result from TestComRegistration method.");
					MessageBox.Show("Failed to get result from TestComRegistration method.","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return ActionResult.Failure;
				}
				session.Log("Successfully called TestComRegistration method. Result: "+oResult.ToString());
			}
			catch(Exception e){
				session.Log("Failed to call TestComRegistration method. ERROR: "+e.Message);
				MessageBox.Show("Failed to call TestComRegistration method. ERROR: "+e.Message,"ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return ActionResult.Failure;
			}
			MessageBox.Show("Successfully installed and tested initialization via COM","SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Information);
			objComTest=null;
			typComTest=null;
			
			#region Run VB script that would call the class
//			try{
//				startInfo = new ProcessStartInfo();
////				startInfo.FileName="\""+Environment.GetFolderPath(Environment.SpecialFolder.System)+"\\cscript.exe\"";
//				startInfo.FileName="\""+Environment.GetFolderPath(Environment.SpecialFolder.System)+"\\wscript.exe\"";//Unlike cscript.exe this one will pop-up the execution result window
//				startInfo.Arguments="\""+strInstallFolder+"\\FlowUtilitiesInitializationTest.vbs\"";
//				//startInfo.FileName="csc.exe";
//				//startInfo.Arguments="\""+Environment.GetFolderPath(Environment.SpecialFolder.System)+"\\cscript.exe\" \""+strDllPathName+"\\FlowUtilitiesInitializationTest.vbs\"";
//				//startInfo.WindowStyle=ProcessWindowStyle.Maximized;
//				startInfo.UseShellExecute=false;
//				startInfo.RedirectStandardError=true;
//				startInfo.CreateNoWindow=true;
//				startInfo.RedirectStandardOutput=true;

//				p = new Process();
//				p.StartInfo = startInfo;
//				p.Start();
//				//System.Text.StringBuilder sb=new StringBuilder();
//				string strOutput=null;
//				while(!p.StandardOutput.EndOfStream){
//					string s=p.StandardOutput.ReadToEnd();
//					if(!string.IsNullOrEmpty(s)){strOutput=s;}
//					//sb.AppendLine(p.StandardOutput.ReadLine());
//				}
//				//p.WaitForExit();
//				while(!p.HasExited){
//					System.Threading.Thread.Sleep(1000);
//				}
//				session.Log("Finished running "+startInfo.FileName+" "+startInfo.Arguments+". ExitCode="+p.ExitCode.ToString());
//				//session.Log("RESULT: "+sb.ToString());
//				session.Log("RESULT: "+strOutput);
//				//session.Log("DLL Initialization test has been successful using "+startInfo.FileName+" "+startInfo.Arguments);
//				//MessageBox.Show(sb.ToString(),"SUCCESS???",MessageBoxButtons.OK,MessageBoxIcon.Information);
//				MessageBox.Show("DLL installation and initialization test had been successful","SUCCESS",MessageBoxButtons.OK,MessageBoxIcon.Information);
//			}
//			catch(Exception e){
//				session.Log("Failed to execute ["+startInfo.FileName+" "+startInfo.Arguments+"]. ExitCode="+p.ExitCode.ToString()+", Error: "+e.Message);
//				MessageBox.Show("Failed to execute ["+startInfo.FileName+" "+startInfo.Arguments+"], Error: "+e.Message,"Failed to register",MessageBoxButtons.OK,MessageBoxIcon.Error);
//				return ActionResult.Failure;
//			}
			#endregion
			
			#endregion

			return ActionResult.Success;
		}
		#endregion
		
		#region GetWixProperty
		private static string GetWixProperty(string propertyId,Session session,string callerName,bool isEmptyOK){
			try{
				string str=session[propertyId];
				if(!isEmptyOK && str.Trim()==""){
					session["WebsiteBootsrapperHelper.GetWixProperty"]=callerName+" EMPTY VALUE supplied for Property ["+propertyId+"]";//Not necessarily an error in some scenarios
					MessageBox.Show("EMPTY VALUE supplied for Property ["+propertyId+"]",callerName,MessageBoxButtons.OK,MessageBoxIcon.Error);//That's the only reliable way as the Log is not always available and CUSTOM_ERROR_MESSAGE also.
					return null;
				}
				return str;
			}
			catch(Exception e){
				session["WebsiteBootsrapperHelper.GetWixProperty"]=callerName+" ERROR. Failed to read Property ["+propertyId+"]: "+e.ToString();
				MessageBox.Show("ERROR. Failed to read Property ["+propertyId+"]: "+e.ToString(),callerName,MessageBoxButtons.OK,MessageBoxIcon.Error);//That's the only reliable way as the Log is not always available and CUSTOM_ERROR_MESSAGE also.
				return null;
			}

		}
		#endregion

	}
}
