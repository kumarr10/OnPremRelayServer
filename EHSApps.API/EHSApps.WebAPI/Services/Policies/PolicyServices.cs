using Entities = EHSApps.WebAPI.PolicyProcedure.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EHSApps.WebAPI.PolicyProcedure.Data;
using System.IO;
using System.Data;
using ConEdison.EHSApps.WebAPI.SPListDataService;
using ConEdison.EHSApps.WebAPI.interfaces;

namespace EHSApps.WebAPI.PolicyProcedure.Services
{
    public class PolicyService: IPolicyService
    {
        public PolicyService()
        {

        }

        public IEnumerable<Entities.Document> SearchPolicies(string queryTxt, string docType ="", string docSubtype="")
        {
            List<Entities.Document> documents = new List<Entities.Document>();
                          
            StringBuilder xmlString = new StringBuilder(@"<QueryPacket xmlns='urn:Microsoft.Search.Query'><Query>" +
                      @"<SupportedFormats><Format revision='1'> urn:Microsoft.Search.Response.Document.Document </Format></SupportedFormats>" + 
                      @"<Context><QueryText language='en-US' type='MSSQLFT'>");
            xmlString.Append(@"SELECT rank, title, path, author, site, filetype FROM SCOPE()");
            string scopeStr = String.Format(@" WHERE ""Scope""='PoliciesPage'");
            xmlString.Append(scopeStr); // user text
            //xmlString.Append(" AND CONTAINS( Title ,'" + queryTxt +"')");
            //xmlString.Append(queryTxt); // user text
            xmlString.Append(@"</QueryText></Context></Query></QueryPacket>");
            string queryXml = xmlString.ToString();
            DataSet docResult = Global.SearchContext.QueryEx(queryXml);
            var dsMetadata = Global.SearchContext.GetSearchMetadata();
            if (docResult != null && docResult.Tables.Count > 0)
            {
                foreach (DataRow row in docResult.Tables[0].Rows)
                {
                    Entities.Document doc = new Entities.Document();
                    doc.DocumentName = row["Title"].ToString();
                    documents.Add(doc);
                }
            }
            return documents;
        }
        public IEnumerable<Entities.DocType> GetDocs(string filterDocType, string lastSyncDate)
        {
            List<Entities.DocType> docTypes = new List<Entities.DocType>();
            List<Entities.Document> documents = null;

            DateTime lastSyncDateVal = DateTime.MinValue;
            var syncDateRes = DateTime.TryParse(lastSyncDate, out lastSyncDateVal);
            //docTypes.Add(new Entities.DocType() { Name = "CEHSP" });
            //docTypes.Add(new Entities.DocType() { Name = "GEHSI" });
            //docTypes.Add(new Entities.DocType() { Name = "Manual" });
            //docTypes.Add(new Entities.DocType() { Name = "Rule Book" });
            //docTypes.Add(new Entities.DocType() { Name = "ED" });
            //docTypes.Add(new Entities.DocType() { Name = "EH&S Guidance" });
            var docTypesResult = from dTypes in Global.Context.PoliciesProceduresPDFDocType
                                 select new Entities.DocType
                                 {
                                     Name = dTypes.Value
                                 };
            docTypes = docTypesResult.ToList();

            if (!string.IsNullOrWhiteSpace(filterDocType) && filterDocType != "0")
                docTypes = docTypes.Where(d => d.Name == filterDocType).ToList();
            
            List<Entities.DocSubType> docSubTypes = new List<Entities.DocSubType>();
            var docSubTypesResult = from dTypes in Global.Context.PoliciesProceduresPDFDocumentType
                                    select new Entities.DocSubType
                                    {
                                        Name = dTypes.Value
                                    };
            docSubTypes = docSubTypesResult.ToList();

            List<Entities.DocSubType> docFilterSubTypes = new List<Entities.DocSubType>();
            foreach (var docType in docTypes)
            {
                docFilterSubTypes = new List<Entities.DocSubType>();
                foreach (var docSubType in docSubTypes.Where(d=>d.Name.StartsWith(docType.Name)))
                {
                    var results = from items in Global.Context.PoliciesProceduresPDF.Expand("DocType").Expand("DocumentType").Expand("ModifiedBy").Expand("DocumentID").Expand("SME")
                                  where items.ContentType != "Folder" &&  items.DocTypeValue == docType.Name && items.DocumentTypeValue == docSubType.Name
                                  select items;
                    documents = new List<Entities.Document>();
                    foreach (var item in results)
                    {                        
                        //if (item.DocType != null && item.DocType.Value == docType.Name && item.DocumentType.Value == docSubType.Name)
                        //{
                            Entities.Document doc = new Entities.Document
                            {
                                ID = item.Id,
                                DocumentName = item.Name,
                                DocumentPath = item.Path,
                                DocType = (item.DocType != null) ? item.DocType.Value : string.Empty,
                                DocSubType = (item.DocumentType != null) ? item.DocumentType.Value : string.Empty,
                                Category = item.Category,
                                Modified = Convert.ToString(item.Modified),
                                ModifiedBy = (item.SME != null) ? item.SME.Name : string.Empty
                            };
                            if(lastSyncDateVal == DateTime.MinValue)
                            documents.Add(doc);
                            else
                            {
                                if (item.Modified >= lastSyncDateVal)
                                    documents.Add(doc);
                            }
                       // }
                    }                  
                    docSubType.Documents = documents.ToArray();
                    docFilterSubTypes.Add(docSubType);
                }
                docType.DocSubTypes = docFilterSubTypes.ToArray();
            }
            return docTypes;
        }

        public  IEnumerable<Entities.Document> SyncDocs(string lastSyncDate)
        {

            List<Entities.Document> documents = null;

            DateTime lastSyncDateVal = DateTime.MinValue;
            var syncDateRes = DateTime.TryParse(lastSyncDate, out lastSyncDateVal);

            var results = from items in Global.Context.PoliciesProceduresPDF.Expand("DocType").Expand("DocumentType").Expand("ModifiedBy").Expand("DocumentID").Expand("SME")
                          where items.ContentType != "Folder" && items.Modified >= lastSyncDateVal
                          select items;
            documents = new List<Entities.Document>();
            foreach (var item in results)
            {
                Entities.Document doc = new Entities.Document
                {
                    ID = item.Id,
                    DocumentName = item.Name,
                    DocumentPath = item.Path,
                    DocType = (item.DocType != null) ? item.DocType.Value : string.Empty,
                    DocSubType = (item.DocumentType != null) ? item.DocumentType.Value : string.Empty,
                    Category = item.Category,
                    Modified = Convert.ToString(item.Modified),
                    ModifiedBy = (item.SME != null) ? item.SME.Name : string.Empty
                };
                //if (lastSyncDateVal == DateTime.MinValue)
                //    documents.Add(doc);
                //else
                //{
                //    if (item.Modified >= lastSyncDateVal)
                //        documents.Add(doc);
                //}
                documents.Add(doc);
            }
            return documents;
        }

        public Stream DownloadDocument(string docName, string docPath)
        {
            EHSPortalDataContext dataContext = Global.Context;
           
            PoliciesProceduresPDFItem listitem = dataContext.PoliciesProceduresPDF.Where(x => x.Name==docName && x.ContentType !="Folder").FirstOrDefault();
            System.Data.Services.Client.DataServiceStreamResponse DataServiceStreamResponse = dataContext.GetReadStream(listitem);
            Stream stream = DataServiceStreamResponse.Stream;      
            return stream;       
        }
      
    }
}
