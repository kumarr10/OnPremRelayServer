using Entities = EHSApps.API.JSSE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using EHSApps.API.JSSE.Data;
using ConEdison.EHSApps.WebAPI.Models;

namespace EHSApps.API.JSSE.Services
{
    public class CategoryService
    {
        public static int AddCategory(Entities.MasterCategory category)
        {
            int result;
            result = JSSECategoryManager.AddCategory(category);
            return result;
        }
        public static int UpdateCategory(Entities.MasterCategory category)
        {
            int result;
            result = JSSECategoryManager.UpdateCategory(category);
            return result;
        }

        public static List<Entities.MasterCategory> GetCategories()
        {
            List<Entities.MasterCategory> categories = new List<Entities.MasterCategory>();
            try
            {
                var dbCategories = JSSECategoryManager.GetCategories();
                foreach (var dbCategory in dbCategories)
                {
                    Entities.MasterCategory category = new Entities.MasterCategory();
                    category.Category_ID = dbCategory.Category_ID;
                    category.Category = dbCategory.Category;
                    category.CategoryDesc = dbCategory.CategoryDesc;
                    category.IsActive = Convert.ToBoolean(dbCategory.Active);
                    category.Status = dbCategory.Active == true ? "Active" : "Inactive";
                    categories.Add(category);
                }
            }
            catch 
            {
                throw ;
            }
            return categories;
        }

        public static List<Entities.MasterCategory> GetCategoriesWithBehaviors()
        {
            List<Entities.MasterCategory> categories = new List<Entities.MasterCategory>();
            try
            {
                var dbCategories = JSSECategoryManager.GetCategories();
                foreach (var dbCategory in dbCategories)
                {
                   
                    Entities.MasterCategory category = new Entities.MasterCategory();
                    category.Category_ID = dbCategory.Category_ID;
                    category.Category = dbCategory.Category;
                    category.CategoryDesc = dbCategory.CategoryDesc;

                    List<Entities.MasterBehavior> behaviors = new List<Entities.MasterBehavior>();
                    List<T_JSSE_Master_CategoryBehavior_Map> dbCatBehMaps = dbCategory.T_JSSE_Master_CategoryBehavior_Map.ToList();
                    foreach (var dbCatBehMap in dbCatBehMaps)
                    {
                        Entities.MasterBehavior behavior = new Entities.MasterBehavior();
                        behavior.Behavior_ID = dbCatBehMap.T_JSSE_Master_Behavior.Behavior_ID;
                        behavior.Behavior = dbCatBehMap.T_JSSE_Master_Behavior.Behavior;
                        behavior.BehaviorDesc = dbCatBehMap.T_JSSE_Master_Behavior.BehaviorDesc;

                        Entities.MasterBehaviorType behaviorType = new Entities.MasterBehaviorType();
                        behaviorType.BehaviorType_ID = dbCatBehMap.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                        behaviorType.BehaviorType = dbCatBehMap.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType;
                        behavior.BehaviorType = behaviorType;

                        behaviors.Add(behavior);
                    }
                    category.Behaviors = behaviors.ToArray();
                    category.IsActive = Convert.ToBoolean(dbCategory.Active);
                    category.Status = dbCategory.Active == true ? "Active" : "Inactive";
                    categories.Add(category);
                }
            }
            catch 
            {
                throw ;
            }
            return categories;
        }
    }
}
