using Entities = EHSApps.WebAPI.PolicyProcedure.Entity;
using System.Collections.Generic;
using System.IO;

namespace ConEdison.EHSApps.WebAPI.interfaces
{
    public interface IPolicyService
    {
        IEnumerable<Entities.Document> SearchPolicies(string queryTxt, string docType = "", string docSubtype = "");

        IEnumerable<Entities.DocType> GetDocs(string filterDocType, string lastSyncDate);

        IEnumerable<Entities.Document> SyncDocs(string lastSyncDate);

        Stream DownloadDocument(string docName, string docPath);

    }
}
