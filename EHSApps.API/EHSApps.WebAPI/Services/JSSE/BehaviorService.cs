using Entities = EHSApps.API.JSSE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using EHSApps.API.JSSE.Data;

namespace EHSApps.API.JSSE.Services
{
    public class BehaviorService
    {
        public static int AddBehavior(Entities.MasterBehavior behavior)
        {
            int result;
            result = JSSEBehaviorManager.AddBehavior(behavior);
            return result;
        }
        public static int UpdateBehavior(Entities.MasterBehavior behavior)
        {
            int result;
            result = JSSEBehaviorManager.UpdateBehavior(behavior);
            return result;
        }

        public static List<Entities.MasterBehavior> GetBehaviors()
        {
            List<Entities.MasterBehavior> behaviors = new List<Entities.MasterBehavior>();
            try
            {
                var dbBehaviors = JSSEBehaviorManager.GetBehaviors();
                foreach (var dbBehavior in dbBehaviors)
                {
                    Entities.MasterBehavior behavior = new Entities.MasterBehavior();
                    behavior.Behavior_ID = dbBehavior.Behavior_ID;
                    behavior.Behavior = dbBehavior.Behavior;
                    behavior.BehaviorDesc = dbBehavior.BehaviorDesc;

                    Entities.MasterBehaviorType behaviorType = new Entities.MasterBehaviorType();
                    behaviorType.BehaviorType_ID = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                    behaviorType.BehaviorType = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType;

                    behavior.BehaviorType = behaviorType;

                    Entities.MasterCategory category = new Entities.MasterCategory();
                    category.Category_ID = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category_ID;
                    category.Category = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category;
                    category.CategoryDesc = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.CategoryDesc;

                    behavior.Category = category;

                    behaviors.Add(behavior);
                }
            }
            catch 
            {
                throw ;
            }
            return behaviors;
        }

        public static List<Entities.MasterBehavior> GetBehaviors(int categoryId, int behaviorTypeId)
        {
            List<Entities.MasterBehavior> behaviors = new List<Entities.MasterBehavior>();
            try
            {
                var dbBehaviors = JSSEBehaviorManager.GetBehaviors();
                foreach (var dbBehavior in dbBehaviors)
                {
                    if (categoryId == dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category_ID)
                    {
                        if (behaviorTypeId == dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType_ID)
                        {
                            Entities.MasterBehavior behavior = new Entities.MasterBehavior();
                            behavior.Behavior_ID = dbBehavior.Behavior_ID;
                            behavior.Behavior = dbBehavior.Behavior;
                            behavior.BehaviorDesc = dbBehavior.BehaviorDesc;

                            Entities.MasterBehaviorType behaviorType = new Entities.MasterBehaviorType();
                            behaviorType.BehaviorType_ID = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                            behaviorType.BehaviorType = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType;

                            behavior.BehaviorType = behaviorType;

                            Entities.MasterCategory category = new Entities.MasterCategory();
                            category.Category_ID = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category_ID;
                            category.Category = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category;
                            category.CategoryDesc = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.CategoryDesc;

                            behavior.Category = category;

                            behavior.IsActive = Convert.ToBoolean(dbBehavior.Active);
                            behavior.Status = dbBehavior.Active == true ? "Active" : "Inactive";

                            behaviors.Add(behavior);
                        }
                        //break;
                    }
                }
            }
            catch 
            {
                throw ;
            }
            return behaviors;
        }

        public static List<Entities.MasterBehavior> GetBehaviors(int categoryId, int behaviorTypeId, int majorGroupId, int orgId)
        {
            List<Entities.MasterBehavior> behaviors = new List<Entities.MasterBehavior>();
            try
            {
                //var dbBehaviors = JSSEBehaviorManager.GetBehaviors();
                var dbBehaviors = JSSEBehaviorManager.GetBehaviors(categoryId, behaviorTypeId);

                foreach (var dbBehavior in dbBehaviors)
                {
                    Entities.MasterBehavior behavior = new Entities.MasterBehavior();
                    behavior.Behavior_ID = dbBehavior.Behavior_ID;
                    behavior.Behavior = dbBehavior.Behavior;
                    behavior.BehaviorDesc = dbBehavior.BehaviorDesc;

                    Entities.MasterBehaviorType behaviorType = new Entities.MasterBehaviorType();
                    behaviorType.BehaviorType_ID = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                    behaviorType.BehaviorType = dbBehavior.T_JSSE_Master_BehaviorType.BehaviorType;

                    behavior.BehaviorType = behaviorType;

                    behavior.MajorGroup_ID = dbBehavior.MajorGroup_ID;
                    behavior.Org_ID = dbBehavior.Org_ID;

                    Entities.MasterCategory category = new Entities.MasterCategory();
                    category.Category_ID = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category_ID;
                    category.Category = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.Category;
                    category.CategoryDesc = dbBehavior.T_JSSE_Master_CategoryBehavior_Map.SingleOrDefault().T_JSSE_Master_Category.CategoryDesc;

                    behavior.Category = category;

                    behavior.IsActive = Convert.ToBoolean(dbBehavior.Active);
                    behavior.Status = dbBehavior.Active == true ? "Active" : "Inactive";

                    behaviors.Add(behavior);
                }
                if (orgId > 0 && behaviorTypeId == 2)
                {
                    behaviors = behaviors.Where(x => x.Org_ID == orgId).ToList();
                }
            }
            catch 
            {
                throw ;
            }
            return behaviors;
        }
    }
}
