using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FlowUtilities
{
     /// <summary>
    /// Class to support Shopify web service integration via the Shopify App Proxy
    /// see https://help.shopify.com/en/api/guides/application-proxies
    /// </summary>
    public class ShopifyProxy
    {
        private string _signature = "";
        /// <summary>
        /// Signature contains the signature value extracted from the query string by the SortParameter method.  It is not publicly writeable
        /// </summary>
        public string Signature
        {
            get => _signature;
        }
        /// <summary>
        /// method to convert the raw query string from a URL into the text that needs to be digitally signed
        /// This is called internally by the Authenticate method
        /// </summary>
        /// <param name="queryString">The raw query string, including all query parameters passed.  The values should still be urlencoded</param>
        /// <param name="signatureKey">this is the name of the parameter that contains the signature.  It will be removed from the sorted string and value returns in getKey</param>
        /// <returns>the sorted query string ready for hashing, minus the parameter mentioned in signatureKey.  The Signature property will also be set to the value of the signature parameter</returns>
        public string SortParameter(string queryString, string signatureKey)
        {
            var result = string.Empty;
            var parseQuery = HttpUtility.ParseQueryString(queryString);

            if (parseQuery.Count == 0)
            {
                _signature = "";
                return result;
            }

            _signature = parseQuery.Get(signatureKey);
            parseQuery.Remove(signatureKey);

            result = string.Join("", new SortedDictionary<string, string>(parseQuery.AllKeys.ToDictionary(k => k, k => parseQuery[k])).Select(x => string.Join("=", x.Key, x.Value)));

            return result;
        }

        /// <summary>
        /// method to calculate the hash of the query string and compare with the signature
        /// It will first sort the query string as per Shopify requirements
        /// </summary>
        /// <param name="queryString">The raw query string, including the signature.  Values should still be urlencoded</param>
        /// <param name="sharedSecret">The shared secret being used for the HmacSHA256 algorithm</param>
        /// <returns>True if signature matches, false otherwise</returns>
        public bool Authenticate(string queryString, string sharedSecret)
        {
            string hmac;
            Encryption enc = new Encryption();
            string sortedString = SortParameter(queryString, "signature");
            hmac = enc.HmacSHA256Hex(sortedString, sharedSecret);
            return String.Compare(hmac, Signature) == 0;
        }
    }
}
