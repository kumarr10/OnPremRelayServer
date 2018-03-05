using System.Collections.Generic;
using System.Linq;
using System.Data;
using ConEdison.EHSApps.WebAPI.Models;

namespace EHSApps.API.JSSE.Data
{
    public class JSSELogManager
    {
        public static int LogtoDatabase(T_JSSE_Log jsseLog)
        {
            int result = 0;
            var context = Global.Context;
            context.T_JSSE_Log.Add(jsseLog);
            result = context.SaveChanges();
            return result;
        }

        public static int AddToConfiguration(T_JSSE_Configuration jsseConfig)
        {
            int result = 0;
            var context = Global.Context;
            context.T_JSSE_Configuration.Add(jsseConfig);
            result = context.SaveChanges();
            return result;
        }

        public static IEnumerable<T_JSSE_Configuration> GetConfiguration(string configType)
        {
            var KPIResults = from c in Global.Context.T_JSSE_Configuration where c.ConfigType == configType select c;
            var KPIEnumerable = KPIResults.AsEnumerable();
            return KPIEnumerable;
        }
    }    
}
