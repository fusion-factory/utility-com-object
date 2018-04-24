using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using FlowUtilities;

namespace FlowUtilitiesTest {
	public partial class Test: Form {
		public Test() {
			InitializeComponent();
		}

        const String tokenKey = "TCcPw1eTIP3jtf2SBWFhZQ==";

        private void Test_Load(object sender,EventArgs e) {
            Encryption E = new Encryption();
            /*
            string original = "{\"active\":\"true\"}";
            string expectedOutput = "ZpvuG8KzgctgU716ZDpkc4nQvkRjY/KhzqnJYkBYg/A=";

            var UTF8 = new System.Text.UTF8Encoding();
            var encString = E.RijndaelEncryptBase64(original, tokenKey, (int)CipherMode.ECB, 128, (int)PaddingMode.PKCS7);

            txtOutput.Text = "Original:   " + original + "\r\n";
            txtOutput.Text += "Expected:   " + expectedOutput + "\r\n";
            txtOutput.Text += "Encrypted:  " + encString + "\r\n";
            txtOutput.Text += "Decrypted:  " + E.RijndaelDecryptBase64(encString, tokenKey, (int)CipherMode.ECB, 128, (int)PaddingMode.PKCS7) + "\r\n";

            Helper H = new Helper();
            H.GenerateInclude("");

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
            */
            //System.Security.Cryptography.X509Certificates.PublicKey publicKey = E.ImportRSAPublicKey("..\\..\\sha256_public.cer");
            //txtOutput.Text += "\r\nPUBLICKEY\r\n";
           //txtOutput.Text += publicKey.Key.ToXmlString(false);
            System.Security.Cryptography.AsymmetricAlgorithm privateKey = E.ImportRSAPrivateKey("..\\..\\private-ff.pfx", "fusionfactory");
            txtOutput.Text += "\r\nKEY ALG\r\n";
            txtOutput.Text += privateKey.KeyExchangeAlgorithm;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)privateKey;
            txtOutput.Text += "\r\nSIGNATURE ALG\r\n";
            txtOutput.Text += rsa.SignatureAlgorithm;
            string signature = E.CreateRSASignatureSHA256("hello world", privateKey);
            txtOutput.Text += "\r\n" + signature;


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
