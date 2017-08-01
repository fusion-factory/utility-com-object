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
            Encryption E = new Encryption();
            Byte[] all_ascii = new Byte[128];
            Byte[] all_utf = new Byte[256];
            for (int i = 0; i < 128; ++i)
                all_ascii[i] = (Byte)i;
            for (int i = 0; i < 256; ++i)
                all_utf[i] = (Byte)i;
            txtOutput.Text = "STARTING\r\n";
            txtOutput.Text += "\r\nEMPTY hashtext\r\n";
            txtOutput.Text += E.SHA256HashHex("");
            txtOutput.Text += "\r\nEMPTY hashtext,key\r\n";
            txtOutput.Text += E.HmacSHA256Hex("", "");
            txtOutput.Text += "\r\nAll ASCII\r\n";
            txtOutput.Text += E.SHA256HashHex(all_ascii);
            txtOutput.Text += "\r\nAll UTF\r\n";
            txtOutput.Text += E.SHA256HashHex(all_utf);
            /*
            System.Security.Cryptography.X509Certificates.PublicKey publicKey = E.ImportRSAPublicKey("..\\..\\test_public.cer");
            txtOutput.Text += "\r\nPUBLICKEY\r\n";
           txtOutput.Text += publicKey.Key.ToXmlString(false);
            System.Security.Cryptography.AsymmetricAlgorithm privateKey = E.ImportRSAPrivateKey("..\\..\\test_cert_key.p12", "notarealpassword");
            txtOutput.Text += "SIGNATURE\r\n";
            string signature = E.CreateRSASignatureMD5("hello world", privateKey);
            txtOutput.Text += signature;
            */


            /*
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
            */
        }
    }
}
