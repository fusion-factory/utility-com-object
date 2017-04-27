using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlowUtilities;

namespace FlowUtilitiesTest {
	public partial class Test: Form {
		public Test() {
			InitializeComponent();
		}

		private void Test_Load(object sender,EventArgs e) {
			//Helper H=new Helper();
			//txtOutput.Text=H.RemoveDiacritics("aácčeéií");
			
			//LockUtil U=new LockUtil();
			//for(int i=0;i<U.GetLockingProcesses(@"E:\Work\Projects\01-Rest\Doc\cXML-Reece.docx");i++){
			//	txtOutput.Text+=U.RetrieveLockingProcess(i);
			//}

			Logger L=new Logger();
			bool blnSuccess=L.Initialize("Data Source=localhost;Initial Catalog=Flow;Integrated Security=True",null,true,true,5);
			txtOutput.Text+=L.LastCallLog;
			blnSuccess=L.Log(0,"error_entity_type","error_entity_id",999,"action","action step",null,"","system_error_detail","message");
			txtOutput.Text+=L.LastCallLog;
		}

		private void cmdAS400_Click(object sender,EventArgs e) {
			txtOutput.Text="Preparing to create AS400";
			try{
				AS400 AS=new AS400();
				bool blnSuccess=false;
				txtOutput.Text+=Environment.NewLine+"AS400 created, about to Initialize";
				blnSuccess=AS.Initialize("IBSE","172.20.30.2","cbennett","P@ssw0rd",7);
				txtOutput.Text+=Environment.NewLine+AS.LastCallLog;
				if(!blnSuccess)return;

				txtOutput.Text+=Environment.NewLine+"About to run InitialProgram";
				blnSuccess=AS.RunInitialProgram("T1");
				txtOutput.Text+=Environment.NewLine+AS.LastCallLog;
				if(!blnSuccess)return;

				txtOutput.Text+=Environment.NewLine+"About to run Disconnect";
				blnSuccess=AS.Disconnect(7);
				txtOutput.Text+=Environment.NewLine+AS.LastCallLog;
				if(!blnSuccess)return;

			}
			catch(Exception ex){
				txtOutput.Text+=Environment.NewLine+ex.Message+ex.StackTrace;
			}
		}
	}
}
