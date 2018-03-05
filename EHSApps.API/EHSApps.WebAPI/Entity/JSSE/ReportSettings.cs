using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace EHSApps.API.JSSE.Entity
{
    [DataContract]
    public class ReportServerSettings
    {
        [DataMember]
        public string ReportServer { get; set; }
        [DataMember]
        public string ReportService { get; set; }
        [DataMember]
        public ReportSettings Report { get; set; }
    }

    [DataContract]
    public class ReportSettings
    {
        [DataMember]
        public string ReportName { get; set; }
        [DataMember]
        public string ReportPath { get; set; }
    }
}
