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
    public class JSSECategoryManager
    {
        public static int AddCategory(MasterCategory category)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "CategoryID", "Category", "CategoryDesc", "CreatedBy", "Active" };
            try
            {
                var JsseValues = new object[] { 0, category.Category, category.CategoryDesc, category.ModifiedBy, category.IsActive };
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateCategory]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }

        public static int UpdateCategory(MasterCategory category)
        {
            int result = 0;
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            //Parameter Names
            string[] JsseParams = new[] { "CategoryID", "Category", "CategoryDesc", "CreatedBy", "Active" };
            try
            {
                var JsseValues = new object[] { category.Category_ID, category.Category, category.CategoryDesc, category.ModifiedBy, category.IsActive };
                var jsseMain = DBGeneric.ExecStoredProcedure(entityBuilder.ProviderConnectionString, "dbo.[usp_JSSE_InsertUpdateCategory]",
                                                JsseParams, JsseValues);
            }
            catch 
            {
                throw ;
            }
            return result;
        }

        public static IEnumerable<T_JSSE_Master_Category> GetCategories()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Category.Include("T_JSSE_Master_CategoryBehavior_Map.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType") select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

    }

}
