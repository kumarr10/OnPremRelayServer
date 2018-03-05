using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using EHSApps.API.JSSE.Entity;
using System.Data.Entity.Core.EntityClient;
using ConEdison.EHSApps.WebAPI.Models;
using EHSApps.API.Utils;
using EHSApps.WebAPI.JSSE.Interfaces;

namespace EHSApps.API.JSSE.Data
{
    public class JSSEMainRepository :IJSSEMainRepository
    {
        public static string jSSEConn = ConfigurationManager.ConnectionStrings["JSSEEntityConn"].ConnectionString;

        public  IEnumerable<vw_JSSE_GetAllJSSEs> GetUserOrgJSSEs(int? org_Id, string user_Id, DateTime? fromDate, DateTime? toDate, bool isOwner)
        {
            IEnumerable<vw_JSSE_GetAllJSSEs> jsseData = null;
            try
            {
                var jSSEResults = from c in Global.Context.vw_JSSE_GetAllJSSEs where c.Org_Id == org_Id select c;
                //jSSEResults = jSSEResults.Where(c => c.IsAnonymous == isAnon);                
                if (!string.IsNullOrEmpty(user_Id))// && !isOwner , For mobile 
                    jSSEResults = jSSEResults.Where(s => s.CreatedBy.ToLower() == user_Id.ToLower() || s.ObserverUserID.ToLower() == user_Id.ToLower());
                if (fromDate != null && fromDate > DateTime.MinValue)
                    jSSEResults = jSSEResults.Where(s => s.JSSEDate >= fromDate);
                if (toDate != null && toDate > DateTime.MinValue)
                    jSSEResults = jSSEResults.Where(s => s.JSSEDate <= toDate);
                if (jSSEResults.Count() > 5000)
                    throw new Exception("Number of JSSEs exceed 5000 limit, please narrow down your search.");
                jsseData = jSSEResults.AsEnumerable();
            }
            catch 
            {
                throw ;
            }
            return jsseData;
        }

        public  IEnumerable<T_JSSE_Master_Category> GetCategoryBehaviors()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Category.Include("T_JSSE_Master_CategoryBehavior_Map").Include("T_JSSE_Master_CategoryBehavior_Map.T_JSSE_Master_Behavior")
                                                                            .Include("T_JSSE_Master_CategoryBehavior_Map.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType")
                                                                      select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public  int UpdateJSSETemp(T_JSSE_Main jsse, byte[] image, string imgName)
        {
            int result = 0;
            var context = Global.Context;
            var jsseModel = context.T_JSSE_Main.Where(js=>js.JSSE_ID== jsse.JSSE_ID).FirstOrDefault();
            if (jsseModel != null)
            {
                jsseModel.Location = jsse.Location;
                //jsseModel.Region_ID = jsse.Region_ID;
                jsseModel.CreatedBy = jsse.ModifiedBy;
                jsseModel.ModifiedBy = jsse.ModifiedBy;
                jsseModel.JSSEDate = jsse.JSSEDate;
                jsseModel.Status = jsse.Status;
                jsseModel.ModifiedDate = DateTime.Now;
                if (image != null && !string.IsNullOrWhiteSpace(imgName))
                {
                    T_JSSE_Attachments attch = new T_JSSE_Attachments();
                    attch.JSSE_ID = jsse.JSSE_ID;
                    attch.FileName = imgName;
                    attch.Title = imgName;
                    attch.Image = image;
                    attch.Active = true;
                    context.T_JSSE_Attachments.Add(attch);
                }
                result = context.SaveChanges();
            }
            return result;
        }
        public  int InsertUpdateJSSE(JSSEMain jsse)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);

            // Observee Data
            var dtObservees = new DataTable();
            dtObservees.Columns.Add("Row_ID", typeof(Int32));
            dtObservees.Columns.Add("Emp_Id", typeof(Int64));
            dtObservees.Columns.Add("User_ID", typeof(String));
            dtObservees.Columns.Add("FirstName", typeof(String));
            dtObservees.Columns.Add("LastName", typeof(String));

            // Observer Data
            var dtObservers = new DataTable();
            dtObservers.Columns.Add("Row_ID", typeof(Int32));
            dtObservers.Columns.Add("Emp_Id", typeof(Int64));
            dtObservers.Columns.Add("User_ID", typeof(String));
            dtObservers.Columns.Add("FirstName", typeof(String));
            dtObservers.Columns.Add("LastName", typeof(String));

            // Supervisor Data
            var dtSupervisors = new DataTable();
            dtSupervisors.Columns.Add("Row_ID", typeof(Int32));
            dtSupervisors.Columns.Add("Emp_Id", typeof(Int64));
            dtSupervisors.Columns.Add("User_ID", typeof(String));
            dtSupervisors.Columns.Add("FirstName", typeof(String));
            dtSupervisors.Columns.Add("LastName", typeof(String));

            // Jsse Categories Data
            var dtCategories = new DataTable();
            dtCategories.Columns.Add("Row_ID", typeof(Int32));
            dtCategories.Columns.Add("Category_ID", typeof(Int32));
            dtCategories.Columns.Add("Rating_ID", typeof(Int32));
            dtCategories.Columns.Add("Behavior_IDs", typeof(String));
            dtCategories.Columns.Add("BehaviorType_IDs", typeof(String));
            dtCategories.Columns.Add("Comment", typeof(String));

            // Jsse Behaviors Data
            var dtBehaviors = new DataTable();
            dtBehaviors.Columns.Add("Row_ID", typeof(Int32));
            dtBehaviors.Columns.Add("Category_ID", typeof(Int32));
            dtBehaviors.Columns.Add("Rating_ID", typeof(Int32));
            dtBehaviors.Columns.Add("Behavior_ID", typeof(Int32));
            dtBehaviors.Columns.Add("BehaviorType_ID", typeof(Int32));
            dtBehaviors.Columns.Add("Comment", typeof(String));

            DataRow dtRow; int jsseMain;

            try
            {
                //in case non-anonymous Observee, get the Observee Major group details
                if (jsse.IsAnonymous == false)
                {
                    jsse.MajorGroup_Id = Convert.ToInt32(jsse.Observees.FirstOrDefault().MajorGroup_Id);
                    jsse.Org_Id = Convert.ToInt32(jsse.Observees.FirstOrDefault().Org_Id);
                    jsse.Dept_Id = Convert.ToInt32(jsse.Observees.FirstOrDefault().Dept_Id);
                    jsse.Section_Id = Convert.ToInt32(jsse.Observees.FirstOrDefault().Section_Id);


                    for (int i = 0; i < jsse.Observees.Length; i++)
                    {
                        dtRow = dtObservees.NewRow();
                        dtRow["Row_ID"] = i + 1;
                        dtRow["Emp_Id"] = jsse.Observees[i].Emp_Id;
                        dtRow["User_ID"] = jsse.Observees[i].User_ID;
                        dtRow["FirstName"] = jsse.Observees[i].FirstName;
                        dtRow["LastName"] = jsse.Observees[i].LastName;
                        dtObservees.Rows.Add(dtRow);
                    }
                }
                else
                {
                    // jsse.JSSEEnteredBy = "Anonymous";
                    dtRow = dtObservees.NewRow();
                    dtRow["Row_ID"] = 1;
                    dtRow["Emp_Id"] = 0;
                    dtRow["User_ID"] = "Anonymous";
                    dtRow["FirstName"] = "Anonymous";
                    dtRow["LastName"] = "Anonymous";
                    dtObservees.Rows.Add(dtRow);
                }

                //in case non-anonymous Observer
                if (jsse.IsOBSRAnonymous == false)
                {
                    for (int i = 0; i < jsse.Observers.Length; i++)
                    {
                        dtRow = dtObservers.NewRow();
                        dtRow["Row_ID"] = i + 1;
                        dtRow["Emp_Id"] = jsse.Observers[i].Emp_Id;
                        dtRow["User_ID"] = jsse.Observers[i].User_ID;
                        dtRow["FirstName"] = jsse.Observers[i].FirstName;
                        dtRow["LastName"] = jsse.Observers[i].LastName;
                        dtObservers.Rows.Add(dtRow);
                    }
                    ////add supervisor ------Hold and comment below untill Supervisor to be shown on UI
                    //if (jsse.IsAnonymous == true)
                    //{
                    //    dtRow = dtSupervisors.NewRow();
                    //    dtRow["Row_ID"] = 1;
                    //    dtRow["Emp_Id"] = 0;
                    //    dtRow["User_ID"] = "Anonymous";
                    //    dtRow["FirstName"] = "Anonymous";
                    //    dtRow["LastName"] = "Anonymous";
                    //    dtSupervisors.Rows.Add(dtRow);
                    //}
                    //else if(jsse.IsSupervsrOBSRSame != 1)
                    //{
                    //    if (jsse.Supervisors.Length > 0)
                    //    {
                    //        dtRow = dtSupervisors.NewRow();
                    //        dtRow["Row_ID"] = 1;
                    //        dtRow["Emp_Id"] = jsse.Supervisors[0].Emp_Id;
                    //        dtRow["User_ID"] = jsse.Supervisors[0].User_ID;
                    //        dtRow["FirstName"] = jsse.Supervisors[0].FirstName;
                    //        dtRow["LastName"] = jsse.Supervisors[0].LastName;
                    //        dtSupervisors.Rows.Add(dtRow);
                    //    }
                    //}
                }
                else
                {
                    dtRow = dtObservers.NewRow();
                    dtRow["Row_ID"] = 1;
                    dtRow["Emp_Id"] = 0;
                    dtRow["User_ID"] = "Anonymous";
                    dtRow["FirstName"] = "Anonymous";
                    dtRow["LastName"] = "Anonymous";
                    dtObservers.Rows.Add(dtRow);

                    ////add supervisor ------Hold and comment below untill Supervisor to be shown on UI
                    //if (jsse.IsSupervsrOBSRSame == 1 || jsse.IsAnonymous==true)
                    //{
                    //    dtRow = dtSupervisors.NewRow();
                    //    dtRow["Row_ID"] = 1;
                    //    dtRow["Emp_Id"] = 0;
                    //    dtRow["User_ID"] = "Anonymous";
                    //    dtRow["FirstName"] = "Anonymous";
                    //    dtRow["LastName"] = "Anonymous";
                    //    dtSupervisors.Rows.Add(dtRow);
                    //}
                    //else
                    //{
                    //    if (jsse.Supervisors.Length > 0)
                    //    {
                    //        dtRow = dtSupervisors.NewRow();
                    //        dtRow["Row_ID"] = 1;
                    //        dtRow["Emp_Id"] = jsse.Supervisors[0].Emp_Id;
                    //        dtRow["User_ID"] = jsse.Supervisors[0].User_ID;
                    //        dtRow["FirstName"] = jsse.Supervisors[0].FirstName;
                    //        dtRow["LastName"] = jsse.Supervisors[0].LastName;
                    //        dtSupervisors.Rows.Add(dtRow);
                    //    }
                    //}                 
                }
                string[] Behavs = new string[2];
                string[] BehavTys = new string[2];

                int rowId = 1;
                int behRowId = 1;
                //int behOrgRowId = 1;
                for (int i = 0; i < jsse.Categories.Length; i++)
                {
                    if (jsse.Categories[i].RatingID != null || (jsse.Categories[i].Comments != null && jsse.Categories[i].Comments != ""))
                    {
                        dtRow = dtCategories.NewRow();
                        dtRow["Row_ID"] = rowId;
                        dtRow["Category_ID"] = jsse.Categories[i].Category_ID;
                        if (jsse.Categories[i].RatingID == "0" || jsse.Categories[i].RatingID == null)
                            dtRow["Rating_ID"] = DBNull.Value;
                        else
                            dtRow["Rating_ID"] = jsse.Categories[i].RatingID;

                        if (jsse.Categories[i].EntBehaviors.Length > 0)
                        {
                            Behavs[0] = string.Join(",", jsse.Categories[i].EntBehaviors.Where(x => x.BehviorChecked).Select(x => x.Behavior_ID.ToString()).ToArray());
                            BehavTys[0] = string.Join(",", jsse.Categories[i].EntBehaviors.Where(x => x.BehviorChecked).Select(x => x.BehaviorType_ID.ToString()).ToArray());
                        }
                        if (jsse.Categories[i].OrgBehaviors.Length > 0)
                        {
                            Behavs[1] = string.Join(",", jsse.Categories[i].OrgBehaviors.Where(y => y.BehviorChecked).Select(y => y.Behavior_ID.ToString()).ToArray());
                            BehavTys[1] = string.Join(",", jsse.Categories[i].OrgBehaviors.Where(x => x.BehviorChecked).Select(x => x.BehaviorType_ID.ToString()).ToArray());
                        }
                        dtRow["Behavior_IDs"] = string.Join(",", Behavs);
                        dtRow["BehaviorType_IDs"] = string.Join(",", BehavTys);
                        dtRow["Comment"] = jsse.Categories[i].Comments;
                        dtCategories.Rows.Add(dtRow);

                        rowId = rowId + 1;
                    }


                    for (int beh = 0; beh < jsse.Categories[i].EntBehaviors.Length; beh++)
                    {
                        var currBeh = jsse.Categories[i].EntBehaviors[beh];
                        if (currBeh.Rating_ID != null)
                        {
                            dtRow = dtBehaviors.NewRow();
                            dtRow["Row_ID"] = behRowId;
                            dtRow["Category_ID"] = jsse.Categories[i].Category_ID;
                            if (currBeh.Rating_ID == null)
                                dtRow["Rating_ID"] = DBNull.Value;
                            else
                                dtRow["Rating_ID"] = currBeh.Rating_ID;

                            dtRow["Behavior_ID"] = currBeh.Behavior_ID;
                            dtRow["BehaviorType_ID"] = currBeh.BehaviorType_ID;
                            dtRow["Comment"] = currBeh.Comments;
                            dtBehaviors.Rows.Add(dtRow);

                            behRowId = behRowId + 1;
                        }
                    }


                    for (int beh = 0; beh < jsse.Categories[i].OrgBehaviors.Length; beh++)
                    {
                        var currBeh = jsse.Categories[i].OrgBehaviors[beh];
                        if (currBeh.Rating_ID != null)
                        {
                            dtRow = dtBehaviors.NewRow();
                            dtRow["Row_ID"] = behRowId;
                            dtRow["Category_ID"] = jsse.Categories[i].Category_ID;
                            if (currBeh.Rating_ID == null)
                                dtRow["Rating_ID"] = DBNull.Value;
                            else
                                dtRow["Rating_ID"] = currBeh.Rating_ID;

                            dtRow["Behavior_ID"] = currBeh.Behavior_ID;
                            dtRow["BehaviorType_ID"] = currBeh.BehaviorType_ID;
                            dtRow["Comment"] = currBeh.Comments;
                            dtBehaviors.Rows.Add(dtRow);

                            behRowId = behRowId + 1;
                        }
                    }
                }

                // Jsse Attachments Data
                var dtAttachments = new DataTable();
                dtAttachments.Columns.Add("Row_ID", typeof(Int32));
                dtAttachments.Columns.Add("Title", typeof(String));
                dtAttachments.Columns.Add("Description", typeof(String));
                dtAttachments.Columns.Add("FileName", typeof(String));
                dtAttachments.Columns.Add("Image", typeof(byte[]));

                for (int i = 0; i < jsse.Attachments.Length; i++)
                {
                    dtRow = dtAttachments.NewRow();
                    dtRow["Row_ID"] = i + 1;
                    dtRow["Title"] = jsse.Attachments[i].Title;
                    dtRow["Description"] = jsse.Attachments[i].Description;
                    dtRow["FileName"] = jsse.Attachments[i].FileName;
                    dtRow["Image"] = Convert.FromBase64String(jsse.Attachments[i].Base64ImageString);
                    dtAttachments.Rows.Add(dtRow);
                }

                List<SqlParameter> sParams = new List<SqlParameter>()
                {
                    new SqlParameter("@JSSE_ID", jsse.JSSE_ID),
                    new SqlParameter("@JobName", jsse.JobName),
                    new SqlParameter("@JobDescription", jsse.JobDescription),
                    new SqlParameter("@IsAnonymous", jsse.IsAnonymous),
                    new SqlParameter("@IsExternal", false),
                    new SqlParameter("@Status", jsse.JSSEStatus),
                    new SqlParameter("@Region_ID", jsse.Region_ID),
                    new SqlParameter("@JSSEDate", jsse.JSSEDate == null ? DateTime.Now : jsse.JSSEDate),
                    new SqlParameter("@MajorGroup_Id", jsse.MajorGroup_Id),
                    new SqlParameter("@Org_Id", jsse.Org_Id),
                    new SqlParameter("@Dept_Id", jsse.Dept_Id),
                    new SqlParameter("@Sect_Id", jsse.Section_Id),
                    new SqlParameter("@JSSEEnteredBy", jsse.JSSEEnteredBy),
                    new SqlParameter("@Location", jsse.Location),
                    new SqlParameter() {ParameterName = "@Categories", SqlDbType = SqlDbType.Structured, Value = dtCategories},
                    new SqlParameter() {ParameterName = "@Behaviors", SqlDbType = SqlDbType.Structured, Value = dtBehaviors},
                    new SqlParameter() {ParameterName = "@Observees", SqlDbType = SqlDbType.Structured, Value = dtObservees},
                    new SqlParameter() {ParameterName = "@Observers", SqlDbType = SqlDbType.Structured, Value = dtObservers},
                    new SqlParameter() {ParameterName = "@Supervisor", SqlDbType = SqlDbType.Structured, Value = dtSupervisors},
                    new SqlParameter() {ParameterName = "@Attachments", SqlDbType = SqlDbType.Structured, Value = dtAttachments}
                };

                jsseMain = DBGeneric.ExecStoredProcedureWithSqlParms(entityBuilder.ProviderConnectionString, "usp_JSSE_InsertUpdateJsse", sParams.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return jsseMain;
        }

        public IEnumerable<T_JSSE_Main> GetJSSE(string jsseId)
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Main.Include("T_JSSE_Master_Region")
                                                                     .Include("T_JSSE_Observer").Include("T_JSSE_Observer.T_JSSE_Hierarchy")
                                                                     .Include("T_JSSE_Observee").Include("T_JSSE_Observee.T_JSSE_Hierarchy")
                                                                     .Include("T_JSSE_Category.T_JSSE_Master_Category")
                                                                     .Include("T_JSSE_Category.T_JSSE_Master_Rating")
                                                                     .Include("T_JSSE_Category.T_JSSE_Comment")
                                                                     .Include("T_JSSE_Category.T_JSSE_Behavior.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType")
                                                                     .Include("T_JSSE_Attachments")
                                                                 where c.JSSE_ID == Convert.ToInt32(jsseId)
                                                                 select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public IEnumerable<T_JSSE_Main> GetJSSE(int jsseId)
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Main.Include("T_JSSE_Master_Region")
                                                                     .Include("T_JSSE_Observer").Include("T_JSSE_Observer.T_JSSE_Hierarchy")
                                                                     .Include("T_JSSE_Observee").Include("T_JSSE_Observee.T_JSSE_Hierarchy")
                                                                     .Include("T_JSSE_Category.T_JSSE_Master_Category")
                                                                     .Include("T_JSSE_Category.T_JSSE_Master_Rating")
                                                                     .Include("T_JSSE_Category.T_JSSE_Comment")
                                                                     .Include("T_JSSE_Category.T_JSSE_Behavior.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType")
                                                                     .Include("T_JSSE_Attachments")
                              where c.JSSE_ID ==jsseId
                              select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public IEnumerable<T_JSSE_Master_Region> GetRegions()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Region select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public IEnumerable<T_JSSE_Master_Rating> GetRatings()
        {
            var jSSEResults = from c in Global.Context.T_JSSE_Master_Rating select c;
            var jSSEEnumerable = jSSEResults.AsEnumerable();
            return jSSEEnumerable;
        }

        public IEnumerable<MajorGroupall> GetMajorGroup(string userId)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            var majorGroupResults = DBGeneric.ExecStoredProcedure<MajorGroupall>(entityBuilder.ProviderConnectionString, "usp_JSSE_MajorGroupByUserId", new[] { "UserId" }, new[] { userId });
            return majorGroupResults;
        }

        public IEnumerable<MajorGroup> GetMajorGroups()
        {
            var jSSEResults = (from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                                                         where c.vp_resp_company_cd == 1
                                                                         select new MajorGroup
                                                                         {
                                                                             MajorGroup_Id = c.vp_cd,
                                                                             MajorGroup_Name = c.vp_level_name
                                                                         }).Distinct();
            var jSSEEnumerable = jSSEResults.ToList();
            return jSSEEnumerable;
        }

        public IEnumerable<Organization> GetOrganizations(string mGroupId)
        {
            var jSSEResults = (from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                                                         where c.vp_cd == mGroupId
                                                                         select new Organization
                                                                         {
                                                                             Org_Id = c.org_cd,
                                                                             Org_Name = c.org_level_name
                                                                         }).Distinct();
            var jSSEEnumerable = jSSEResults.ToList();
            return jSSEEnumerable;
        }

        public IEnumerable<Department> GetDepartments(string orgId)
        {
            var jSSEResults = (from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                                                         where c.org_cd == orgId
                                                                         select new Department
                                                                         {
                                                                             Dept_Id = c.dept_cd,
                                                                             Dept_Name = c.dept_level_name
                                                                         }).Distinct();
            var jSSEEnumerable = jSSEResults.ToList();
            return jSSEEnumerable;
        }

        public IEnumerable<Section> GetSections(string deptId)
        {
            var jSSEResults = (from c in Global.Context.EDW_COPY_vHIERARCHY_CURR_SEC_ALL_LEVELS
                                                                         where c.dept_cd == deptId
                                                                         select new Section
                                                                         {
                                                                             Section_Id = c.sect_cd,
                                                                             Section_Name = c.sect_level_name
                                                                         }).Distinct();
            var jSSEEnumerable = jSSEResults.ToList();
            return jSSEEnumerable;
        }

        public int AddModifyJsseCategory(JSSEMain jsse, JSSECategory category)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            var majorGroupResults = DBGeneric.ExecInsertUpdateStoredProcedure(entityBuilder.ProviderConnectionString, "usp_JSSE_InsertUpdateJsseCategory", new[] { "JSSE_ID", "Hierarchy_ID", "Category_ID" }, new[] { Convert.ToString(jsse.JSSE_ID), Convert.ToString(jsse.Hierarchy_ID), Convert.ToString(category.Category_ID) });
            return majorGroupResults;
        }

        public IEnumerable<JSSEMain> AddModifyJsseMain(JSSEMain jsse)
        {
            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder(Global.jSSEConn);
            var jsseMain = DBGeneric.ExecInsertUpdateStoredProcedure<JSSEMain>(entityBuilder.ProviderConnectionString, "usp_JSSE_InsertUpdateJsseMain", new[] { "UserId", "JobName", "JobDescription" }, new[] { "1", jsse.JobName, jsse.JobDescription });
            return jsseMain;
        }
    }
    public static class Global
    {
        public static string jSSEConn = ConfigurationManager.ConnectionStrings["JSSEEntityConn"].ConnectionString;

        public static JSSEEntityConn Context
        {
            get
            {
                //string ocKey = "key_" + HttpContext.Current.GetHashCode().ToString("x");
                //if (!HttpContext.Current.Items.Contains(ocKey))
                //    HttpContext.Current.Items.Add(ocKey, new JSSEEntityConn());

                return new JSSEEntityConn();
            }
        }

    }


    //public partial class KPIEntityConn : DbContext
    //{
    //    //public KPIEntityConn(string connString)
    //    //    : base("name=KPIEntityConn")
    //    //{
    //    //    this.eDatabase.Connection.ConnectionString = connString;
    //    //}


    //    /// <summary>
    //    /// Initialize a new KPIEntityConn object.
    //    /// </summary>
    //    public KPIEntityConn(string connectionString) :
    //        base(connectionString, "KPIEntityConn")
    //    {
    //        this.Configuration.LazyLoadingEnabled = false;
    //    }
    //}
}
