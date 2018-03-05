using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EHSApps.WebAPI.PolicyProcedure.Services;
using System.Web;
using EHSApps.WebAPI.PolicyProcedure.Entity;
using System.IO;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Net.Http;
using ConEdison.EHSApps.WebAPI.interfaces;

namespace EHSApps.WebAPI.PolicyProcedure.Controllers
{
    [RoutePrefix("api/policies")]
    public class PolicyController : ApiController
    {
        #region "All GET Service Methods"
        //[Authorize]
        [HttpGet]
        [Route("ByType/{doctype}")]
        public DocType[] GetDocs(string doctype="", string lastSyncDate = "")
        {
            IPolicyService policyService = new PolicyService();
            if (!string.IsNullOrWhiteSpace(doctype))
                doctype = doctype.ToUpper();
            return policyService.GetDocs(doctype, lastSyncDate).ToArray();
        }

        [HttpGet]
        [Route("SyncDocs")]
        public Document[] SyncDocs(string lastSyncDate = "")
        {
            IPolicyService policyService = new PolicyService();
            return policyService.SyncDocs(lastSyncDate).ToArray();
        }

        [HttpGet]
        [Route("SearchDocs/{queryTxt}")]
        public Document[] SearchDocs(string queryTxt)
        {
            IPolicyService policyService = new PolicyService();
            return policyService.SearchPolicies(queryTxt, "","").ToArray();
        }

        [HttpPost]
        [Route("DownloadDocStream")]
        public byte[] DownloadDocument([FromBody]DownloadDoc doc)
        {
            IPolicyService policyService = new PolicyService(); 
            //DocID="EHSDOC-4-1"  
            byte[] fileBytes;
            if (doc != null)
            {
                var stream = policyService.DownloadDocument(doc.Name, doc.Path);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    fileBytes = ms.GetBuffer();
                   // response.Content.Headers.ContentLength = ms.Length;
                }
                return fileBytes;
            }
            else
                return null;
        }

        [HttpPost]        
        [Route("DownloadFile")]
        public HttpResponseMessage DownloadFile([FromBody]DownloadDoc doc)
        {
            //byte[] fileBytes = null;
            IPolicyService policyService = new PolicyService();
            Stream stream = null;
            if (doc != null)
                stream = policyService.DownloadDocument(doc.Name, doc.Path);
            else             
                return new HttpResponseMessage(HttpStatusCode.BadRequest);          
          
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                response.Content = new ByteArrayContent(ms.GetBuffer());
                response.Content.Headers.ContentLength = ms.Length;
            }
            
            response.Headers.AcceptRanges.Add("bytes");
            response.StatusCode =  HttpStatusCode.OK;
            response.Content.Headers.ContentDisposition
              = new ContentDispositionHeaderValue("attachment");         
            response.Content.Headers.ContentType
              = new MediaTypeHeaderValue("application/octet-stream");
            //response.Content.Headers.ContentLength = stream.Length;
            //response.Content.Headers.ContentLength = contentInfo.Length;

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            //response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/pdf");
            
            response.Content.Headers.ContentDisposition.FileName = doc.Name;
          
            return response;
        }
            
        #endregion

        #region "All POST Service Methods"

       

        #endregion
      
    }
}
