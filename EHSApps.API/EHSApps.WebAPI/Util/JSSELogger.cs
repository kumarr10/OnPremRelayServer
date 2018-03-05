using EHSApps.API.JSSE.Data;
using EHSApps.API.JSSE.Entity;
using ConEdison.EHSApps.WebAPI.Models;
using System;
using System.Web;

namespace EHSApps.API.JSSE.Utils
{
    public class JSSELogger
    {
        public int LogtoDatabase(JSSELog jsse)
        {
            int result = 0;
            T_JSSE_Log dbJSSE = new T_JSSE_Log();
            dbJSSE.Message = jsse.Message;
            dbJSSE.StackTrace = jsse.Stacktrace;
            dbJSSE.EventType = jsse.EventType;
            dbJSSE.EventCategory = jsse.EventCategory;
            dbJSSE.EventFunction = jsse.EventFunction;
            dbJSSE.AppURL = jsse.AppURL;
            dbJSSE.CreatedBy = jsse.UserId;
            dbJSSE.CreatedDate = DateTime.Now;
            dbJSSE.Active = true;
            result = JSSELogManager.LogtoDatabase(dbJSSE);
            return result;
        }
    }
}
