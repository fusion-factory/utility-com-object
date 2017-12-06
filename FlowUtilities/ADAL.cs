using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net;

namespace FlowUtilities {
	/// <summary>
	/// Class to work with ADAL
	/// </summary>
	public class ADAL {
		
		#region LastCallLog
		private string _LastCallLog;
		/// <summary>
		/// Immediately query this property after each call or it will be overwritten by the next call
		/// </summary>
		public string LastCallLog{
			get{return _LastCallLog;}
		}
		#endregion
		
		#region AuthorizationHeader
		private string _AuthorizationHeader;
		/// <summary>
		/// Immediately query this property after call to GetAuthHeader
		/// </summary>
		public string AuthorizationHeader{
			get{return _AuthorizationHeader;}
		}
		#endregion
		
		#region AuthorizationToken
		private string _AuthorizationToken;
		/// <summary>
		/// Immediately query this property after call to GetAuthHeader
		/// </summary>
		public string AuthorizationToken{
			get{return _AuthorizationToken;}
		}
		#endregion

		#region GetAuthHeaderBySharedSecret
		/// <summary>
		/// Gets ADAL authorisation Header and Token using shared secret.
		/// </summary>
		/// <param name="resourceName">Identifier present within the tenant for the B2B service which will be provided to the Partner.  This is a static alpha-numeric string that is mastered by Microsoft.</param>
		/// <param name="tenantInformationURL">Something like https://login.windows-ppe.net/{YourTenantDomain}/oauth2/token?api-version=1.0 (Pre-Production) or https://login.windows.net/{YourTenantDomain}/oauth2/token?api-version=1.0 with {YourTenantDomain} replaced with your value</param>
		/// <param name="clientId">This is the unique identifier for your provisioned application/account to call B2B service.  It is given back when you provision with LPC.</param>
		/// <param name="sharedSecret">Shared secret</param>
		/// <returns>0 for success, 1 for general error, 2 for AuthenticationException, 3 for Certificate not found</returns>
		public int GetAuthHeaderBySharedSecret(string resourceName,string tenantInformationURL,string clientId, string sharedSecret){
			_LastCallLog="";
			_AuthorizationHeader="";
			_AuthorizationToken="";

			try{
				_LastCallLog+="Creating Authentication Context. Microsoft.IdentityModel.Clients.ActiveDirectory must be installed\r\n";
				AuthenticationContext authenticationContext = new AuthenticationContext(authority: tenantInformationURL, validateAuthority: false);

                //Acquire the token by passing clientId and client certificate.
				_LastCallLog+="About to get AuthenticationResult for clientId=["+clientId+"], sharedSecret=["+sharedSecret+"]\r\n";
                AuthenticationResult authenticationResult = authenticationContext.AcquireToken(resource: resourceName,credential:new ClientCredential(clientId,sharedSecret));
                
				// The authorization header contains the scheme and the token
                _AuthorizationHeader = authenticationResult.CreateAuthorizationHeader();

                // Get the JWT Token only
                _AuthorizationToken = authenticationResult.AccessToken;
				_LastCallLog+="Successfully got AuthenticationResult, all good";
                return 0;
            }
            catch (ActiveDirectoryAuthenticationException ex){
                string strErr="ERROR: Acquiring a token failed (ActiveDirectoryAuthenticationException) with the following error: "+ex.Message+ex.StackTrace; 
                if (ex.InnerException != null){
                    strErr+="\r\n\r\nError detail: "+ex.InnerException.Message+ex.InnerException.StackTrace;
                }
				_LastCallLog=strErr+Environment.NewLine+_LastCallLog;
				return 2;
            }
            catch (Exception ex){
                string strErr="ERROR: "+ex.Message+ex.StackTrace; 
                if (ex.InnerException != null){
                    strErr+="\r\n\r\nError detail: "+ex.InnerException.Message+ex.InnerException.StackTrace;
                }
				_LastCallLog=strErr+Environment.NewLine+_LastCallLog;
				return 1;
            }

		}
		#endregion

		#region GetAuthHeaderByCertificate
		/// <summary>
		/// Gets ADAL authorisation Header and Token using Certificate. Read NGVL Partner Service Onboarding 6.2.9
		/// </summary>
		/// <param name="resourceName">Identifier present within the tenant for the B2B service which will be provided to the Partner.  This is a static alpha-numeric string that is mastered by Microsoft.</param>
		/// <param name="tenantInformationURL">Something like https://login.windows-ppe.net/{YourTenantDomain}/oauth2/token?api-version=1.0 (Pre-Production) or https://login.windows.net/{YourTenantDomain}/oauth2/token?api-version=1.0 with {YourTenantDomain} replaced with your value</param>
		/// <param name="clientId">This is the unique identifier for your provisioned application/account to call B2B service.  It is given back when you provision with LPC.</param>
		/// <param name="certificateThumbPrint">The X509 Certificate that you submitted to AAD during the provisioning process.  It is created, owned and mastered by the Partner</param>
		/// <returns>0 for success, 1 for general error, 2 for AuthenticationException, 3 for Certificate not found</returns>
		public int GetAuthHeaderByCertificate(string resourceName,string tenantInformationURL,string clientId, string certificateThumbPrint){
			_LastCallLog="";
			_AuthorizationHeader="";
			_AuthorizationToken="";

			X509Store store=null;
			AuthenticationContext authenticationContext=null;
			AuthenticationResult authenticationResult=null;
			try{
				_LastCallLog+=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+Environment.NewLine;
				//_LastCallLog+="Set UseDefaultCredentials=true\r\n";
				//System.Net.WebProxy.GetDefaultProxy().UseDefaultCredentials=true;//Hopefully that will affect Microsoft.IdentityModel.Clients.ActiveDirectory

				#region Get certificate.  It assumes the cert is installed in LocalMachine cert store
				_LastCallLog+="Creating X509Store\r\n";
				store = new X509Store(StoreName.My,StoreLocation.LocalMachine);//TODO: Change StoreLocation
				_LastCallLog+="Opening Certificates Store\r\n";
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
				_LastCallLog+="Casting Certificates\r\n";
				var list = store.Certificates.Cast<X509Certificate2>().ToList();
				_LastCallLog+="Finding Certificate\r\n";
				var clientCertificate = list.Find(c => c.Thumbprint.Equals(certificateThumbPrint, StringComparison.InvariantCultureIgnoreCase));
				if(clientCertificate==null){
					_LastCallLog="ERROR: Could not find Certificate with certificateThumbPrint=["+certificateThumbPrint+"] in LocalMachine Store. Make sure that the Certificate is installed into that store\r\n"+_LastCallLog;
					return 3;
				}
				#endregion

				_LastCallLog+="Creating Authentication Context. Microsoft.IdentityModel.Clients.ActiveDirectory must be installed\r\n";
				authenticationContext = new AuthenticationContext(authority: tenantInformationURL, validateAuthority: false);

                //Acquire the token by passing clientId and client certificate.
				DateTime dtmBefore=DateTime.Now;
				_LastCallLog+="About to get AuthenticationResult for clientId=["+clientId+"], clientCertificate=["+clientCertificate+"]\r\n";
                authenticationResult = authenticationContext.AcquireToken(resource: resourceName,credential: new X509CertificateCredential(ownerId: clientId,certificate: clientCertificate));
 				_LastCallLog+="AuthenticationResult received in "+DateTime.Now.Subtract(dtmBefore).TotalMilliseconds.ToString("######,###")+" milliseconds\r\n";
               
				// The authorization header contains the scheme and the token
				string strHeader=authenticationResult.CreateAuthorizationHeader();//Unnecessary step, just for data3 peace of mind...
                _AuthorizationHeader = strHeader;
				_LastCallLog+="Created AuthorizationHeader ["+strHeader+"]\r\n";

                // Get the JWT Token only
                string strToken = authenticationResult.AccessToken;//Unnecessary step, just for data3 peace of mind...
				_AuthorizationToken=strToken;
				_LastCallLog+="Successfully got Authentication Token ["+strToken+"], all good";
                return 0;
            }
            catch (ActiveDirectoryAuthenticationException ex){
                string strErr="ERROR: Acquiring a token failed (ActiveDirectoryAuthenticationException) with the following error: "+ex.Message+ex.StackTrace; 
                if (ex.InnerException != null){
                    strErr+="\r\n\r\nError detail: "+ex.InnerException.Message+ex.InnerException.StackTrace;
                }
				_LastCallLog=strErr+Environment.NewLine+_LastCallLog;
				return 2;
            }
            catch (Exception ex){
                string strErr="ERROR: "+ex.Message+ex.StackTrace; 
                if (ex.InnerException != null){
                    strErr+="\r\n\r\nError detail: "+ex.InnerException.Message+ex.InnerException.StackTrace;
                }
				_LastCallLog=strErr+Environment.NewLine+_LastCallLog;
				return 1;
            }
			finally{
				store=null;
				authenticationResult=null;
				authenticationContext=null;
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}

		}
		#endregion



	}
}
