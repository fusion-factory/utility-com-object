using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace FlowUtilities {
	public class LockUtil {//http://bc-programming.com/blogs/2014/02/determining-what-processes-are-locking-a-file/
		#region Declarations
		List<Process> _ProcessesList=null;
		[StructLayout(LayoutKind.Sequential)]
		struct RM_UNIQUE_PROCESS{
			public int dwProcessId;
			public System.Runtime.InteropServices.ComTypes.FILETIME ProcessStartTime;
		}
 
		const int RmRebootReasonNone = 0;
		const int CCH_RM_MAX_APP_NAME = 255;
		const int CCH_RM_MAX_SVC_NAME = 63;
 
		enum RM_APP_TYPE{
			RmUnknownApp = 0,
			RmMainWindow = 1,
			RmOtherWindow = 2,
			RmService = 3,
			RmExplorer = 4,
			RmConsole = 5,
			RmCritical = 1000
		}
 
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		struct RM_PROCESS_INFO{
			public RM_UNIQUE_PROCESS Process;
 
			 [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_APP_NAME + 1)]
			public string strAppName;
			 [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCH_RM_MAX_SVC_NAME + 1)]
			public string strServiceShortName;
			public RM_APP_TYPE ApplicationType;
			public uint AppStatus;
			public uint TSSessionId;
			 [MarshalAs(UnmanagedType.Bool)]
			public bool bRestartable;
		}
		
		[DllImport("rstrtmgr.dll", CharSet = CharSet.Unicode)]
		static extern int RmRegisterResources(uint pSessionHandle,
											  UInt32 nFiles,
											  string []  rgsFilenames,
											  UInt32 nApplications,
											   [In]  RM_UNIQUE_PROCESS []  rgApplications,
											  UInt32 nServices,
											  string []  rgsServiceNames);
 
		[DllImport("rstrtmgr.dll", CharSet = CharSet.Auto)]
		static extern int RmStartSession(out uint pSessionHandle, int dwSessionFlags, string strSessionKey);
		[DllImport("rstrtmgr.dll")]
		static extern int RmEndSession(uint pSessionHandle);
		[DllImport("rstrtmgr.dll")]
		static extern int RmGetList(uint dwSessionHandle,
									out uint pnProcInfoNeeded,
									ref uint pnProcInfo,
									 [In, Out]  RM_PROCESS_INFO []  rgAffectedApps,
									ref uint lpdwRebootReasons);
 
		#endregion
		
		/// <summary>
		/// Find out what process or processes have a lock on the specified file.
		/// </summary>
		/// <param name="filePathName"></param>
		/// <returns></returns>
		public int GetLockingProcesses(String filePathName){
			return GetLockingProcesses(new string []  { filePathName });
		}

		#region GetLockingProcesses
		/// <summary>
		/// Find out what process or processes have a lock on the specified files.
		/// </summary>
		/// <param name="filePathNames">List of files PathNames to check for locking.</param>
		/// <returns>Count of processes found locking the files</returns>
		public int GetLockingProcesses(string []  filePathNames){
			uint handle;
			string key = Guid.NewGuid().ToString();
			_ProcessesList = new List<Process>();
 
			int res = RmStartSession(out handle, 0, key);
			if (res != 0) throw new Exception("Restart Manager Session could not be started.");
 
			try{
				const int ERROR_MORE_DATA = 234;
				uint pnProcInfoNeeded = 0,
					 pnProcInfo = 0,
					 lpdwRebootReasons = RmRebootReasonNone;
 
				string []  resources = filePathNames; // Just checking on one resource.
 
				res = RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);
 
				if (res != 0) throw new Exception("Could not register resource.");
 
           
				//First call to rmGetList() returns the total numberof processes, but when called
				//again the actual number may have increased.
				res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);
 
				if (res == ERROR_MORE_DATA){
					// This takes me back to FindNextFile() in a way. Except we can grab all the results
					// simultaneously, which is nice.
					RM_PROCESS_INFO []  processInfo = new RM_PROCESS_INFO [pnProcInfoNeeded] ;
					pnProcInfo = pnProcInfoNeeded;
 
					// Get the list
					res = RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
					if (res == 0){
						_ProcessesList = new List<Process>((int)pnProcInfo);
 
						//Enumerate the results…
						for (int i = 0; i < pnProcInfo; i++){
							try	{
								//we have a ProcessID, and the Process has a static for retrieving
								//a Process object by ID.
								//This can fail if we don’t have permission to access the process as well
								//as if the Process ID in question has since terminated.
								_ProcessesList.Add(Process.GetProcessById(processInfo [i] .Process.dwProcessId));
							}
                       
							catch (ArgumentException) { }
						}
					}
					else throw new Exception("Failed to list Program(s) locking given resources.");
				}
				else if (res != 0) throw new Exception("Could not list processes locking resources. Failed to get size of result.");
			}
			finally{
				//End the session.
				RmEndSession(handle);
			}
 
			return _ProcessesList.Count;
		}
		#endregion

		public string RetrieveLockingProcess(int processIndex){
			#region Sanity Check
			if(_ProcessesList==null){
				throw new Exception("Internally-stored list is not initialized. You need to call GetLockingProcesses method first and it should return positive number that represents number of Processes");
			}
			if(processIndex<0 || processIndex>_ProcessesList.Count-1){
				throw new Exception("Internally-stored list is "+_ProcessesList.Count.ToString()+" long and the requested index was "+processIndex.ToString()+". Indexes are zero-based when requesting Processes from this object");
			}
			#endregion
			try{
				Process P=_ProcessesList[processIndex];
				return "Name=["+P.ProcessName+"], Machine=["+P.MachineName+"], Window title=["+((P.MainWindowTitle==null)?"":P.MainWindowTitle)+"], Handle=["+P.Handle.ToString()+"], Id=["+P.Id.ToString()+"]";
			}
			catch(Exception e){
				throw new Exception("Exception accessing Process with index "+processIndex.ToString(),e);
			}
		}

	}


}
