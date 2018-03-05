using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using EHSApps.API.JSSE.Entity;
using System.Data.Entity.Core.EntityClient;
using ConEdison.EHSApps.WebAPI.Models;
using EHSApps.API.Utils;

namespace EHSApps.API.JSSE.Data
{
    public class JSSEBehaviorManager
    {
        public static int AddBehavior(MasterBehavior behavior)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "BehaviorID", "Behavior", "BehaviorDesc", "CreatedBy", "Active", "CategoryID", "BehaviorTypeID", "MajorGroupID", "OrgID" };
            try
            {
                var JsseValues = new object[] { 0, behavior.Behavior, behavior.BehaviorDesc, behavior.ModifiedBy, behavior.IsActive, behavior.Category.Category_ID, 
                                                behavior.BehaviorType.BehaviorType_ID, behavior.MajorGroup_ID, behavior.Org_ID};
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateBehavior]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw;
            }
            return result;
        }

        public static int UpdateBehavior(MasterBehavior behavior)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "BehaviorID", "Behavior", "BehaviorDesc", "CreatedBy", "Active", "CategoryID", "BehaviorTypeID", "MajorGroupID", "OrgID" };
            try
            {
                var JsseValues = new object[] { behavior.Behavior_ID, behavior.Behavior, behavior.BehaviorDesc, behavior.ModifiedBy, behavior.IsActive, behavior.Category.Category_ID,
                                                behavior.BehaviorType.BehaviorType_ID, behavior.MajorGroup_ID, behavior.Org_ID};
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateBehavior]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }

        public static IEnumerable<T_JSSE_Master_Behavior> GetBehaviors()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Behavior.Include("T_JSSE_Master_CategoryBehavior_Map.T_JSSE_Master_Category").Include("T_JSSE_Master_BehaviorType") select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public static IEnumerable<T_JSSE_Master_Behavior> GetBehaviors(int categoryId, int behaviorTypeId)
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Behavior.Include("T_JSSE_Master_CategoryBehavior_Map.T_JSSE_Master_Category").Include("T_JSSE_Master_BehaviorType")
                                                                     where c.T_JSSE_Master_CategoryBehavior_Map.FirstOrDefault().T_JSSE_Master_Category.Category_ID == categoryId &&
                                                                           c.T_JSSE_Master_BehaviorType.BehaviorType_ID == behaviorTypeId
                                                                     select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

    }
}
