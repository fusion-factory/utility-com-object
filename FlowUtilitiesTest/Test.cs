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
			Helper H=new Helper();
			txtOutput.Text=H.RemoveDiacritics("aácčeéií");
			
		    LockUtil U=new LockUtil();
			for(int i=0;i<U.GetLockingProcesses(@"C:\GitHub\REST-API\Doc\FlowRest-InstallationGuide.docx");i++){
				txtOutput.Text+=U.RetrieveLockingProcess(i);
			}

			Logger L=new Logger();
			bool blnSuccess=L.Initialize("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Flow_Test_Stage;Integrated Security=True",null,true,true,5);
			txtOutput.Text+=L.LastCallLog;
			blnSuccess=L.Log(0,"error_entity_type","error_entity_id",999,"action","action step",null,"","system_error_detail","message");
			txtOutput.Text+=L.LastCallLog;
		}
	}
}
