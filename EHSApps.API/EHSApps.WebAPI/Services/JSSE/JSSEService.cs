using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EHSApps.API.JSSE.Data;
using EHSApps.API.JSSE.Entity;
using Coned.EHSApps.JSSE.Library.Data;
using System.IO;
using EHSApps.WebAPI.JSSE.Interfaces;
using ConEdison.EHSApps.WebAPI.Models;

namespace EHSApps.API.JSSE.Services
{
    public class JSSEService : IJSSEService
    {
        IJSSEMainRepository jsseRepo;
        public JSSEService()
        {
            jsseRepo = new JSSEMainRepository();
        }
        public List<Region> GetRegions()
        {
            List<Region> oRegions = new List<Region>();
            var dbRegions = jsseRepo.GetRegions().ToList();
            foreach (var dbRegion in dbRegions)
            {
                Region oRegion = new Region();
                oRegion.Region_ID = dbRegion.Region_ID;
                oRegion.RegionName = dbRegion.Region;
                oRegions.Add(oRegion);
            }
            return oRegions;
        }

        public List<JSSEMain> GetUserJSSEs(JSSESearch jsseFilter)
        {
            List<JSSEMain> oJsses = new List<JSSEMain>();
            try
            {
                if (jsseFilter != null && !string.IsNullOrWhiteSpace(jsseFilter.UserName))
                {
                    bool isOwner = false;
                    jsseFilter.UserName = "coned\\" + jsseFilter.UserName;//jsseFilter.UserName.ToLower().Replace("coned\\", "");
                    string[] ownerOrgs = jsseFilter.OwnerOrgIds.Split(',');
                    if (ownerOrgs.Any(o => o == jsseFilter.Org_Id.ToString()))
                        isOwner = true;
                    DateTime fromDate, toDate;
                    DateTime.TryParse(jsseFilter.FromDate, out fromDate);
                    DateTime.TryParse(jsseFilter.ToDate, out toDate);
                    var dbJsses = jsseRepo.GetUserOrgJSSEs(jsseFilter.Org_Id, jsseFilter.UserName, fromDate, toDate, isOwner);
                    foreach (var dbJSSE in dbJsses)
                    {
                        JSSEMain oJsse = new JSSEMain();
                        oJsse.JSSE_ID = dbJSSE.JSSE_ID;
                        oJsse.Base64_JSSE_ID = dbJSSE.JSSE_ID.ToString();//Crypto.EncryptStringAES(Convert.ToString(dbJSSE.JSSE_ID), "JSSE");
                        oJsse.JobName = dbJSSE.JobName;
                        oJsse.JobDescription = dbJSSE.JobDescription;
                        oJsse.JSSEDate = dbJSSE.JSSEDate;
                        oJsse.Region_ID = dbJSSE.Region_ID;
                        oJsse.Region = dbJSSE.RegionName;
                        oJsse.Location = dbJSSE.Location;
                        oJsse.JSSEEnteredBy = dbJSSE.CreatedBy.ToLower().Replace("coned\\", "");
                        oJsse.CreatedDate = dbJSSE.CreatedDate;
                        oJsse.JSSEStatus = dbJSSE.Status;
                        oJsse.IsAnonymous = dbJSSE.IsAnonymous;
                        oJsse.IsExternal = dbJSSE.IsExternal;
                        oJsse.Org_Id = dbJSSE.Org_Id;
                        oJsse.MajorGroup_Id = dbJSSE.MajorGroup_Id;
                        oJsse.AttachmentCount = dbJSSE.AttachmentCount;
                        if (dbJSSE.Observees != null)
                        {
                            var dbObservees = dbJSSE.Observees.Split(',');
                            List<UserInfo> observees = new List<UserInfo>();
                            foreach (var dbObsrvee in dbObservees)
                            {
                                UserInfo observee = new UserInfo();
                                observee.FullName = dbObsrvee;
                                observees.Add(observee);
                            }
                            oJsse.Observees = observees.ToArray();
                        }
                        oJsse.IsCreator = false;
                        if (dbJSSE.Observer != null)
                        {
                            oJsse.Observer = new UserInfo() { FullName = dbJSSE.Observer, User_ID = dbJSSE.ObserverUserID.ToLower().Replace("coned\\", "") };
                            if (oJsse.Observer.User_ID == jsseFilter.UserName.ToLower().Replace("coned\\", ""))
                                oJsse.IsCreator = true;
                        }
                        if (oJsse.JSSEEnteredBy == jsseFilter.UserName.ToLower().Replace("coned\\", ""))
                            oJsse.IsCreator = true;
                        if (ownerOrgs.Any(o => o == oJsse.Org_Id.ToString()))//&& oJsse.IsAnonymous == false
                            oJsse.IsCreator = true;
                        oJsses.Add(oJsse);
                    }
                }
            }
            catch 
            {
                throw;
            }
            return oJsses;
        }

        public JSSEMain GetJSSE(int jsseId)
        {
            JSSEMain oJsse = new JSSEMain();
            try
            {
                var dbJsses = jsseRepo.GetJSSE(jsseId).ToList();
                foreach (var dbJSSE in dbJsses)
                {
                    oJsse.IsAnonymous = dbJSSE.IsAnonymous;
                    //Observers
                    List<UserInfo> oObservers = new List<UserInfo>();
                    List<UserInfo> oSupervisors = new List<UserInfo>();
                    var selObservers = string.Empty;
                    var selSupervisors = string.Empty;
                    foreach (var dbObserver in dbJSSE.T_JSSE_Observer.Where(j => j.Active == true))
                    {
                        UserInfo observer = new UserInfo();
                        observer.Emp_Id = Convert.ToInt64(dbObserver.Emp_ID);
                        observer.FirstName = dbObserver.FirstName;
                        observer.LastName = dbObserver.LastName;
                        observer.FullName = dbObserver.LastName + " , " + dbObserver.FirstName;
                        observer.MajorGroup_Id = Convert.ToString(dbObserver.T_JSSE_Hierarchy.MajorGroup_Id);
                        observer.Org_Id = Convert.ToString(dbObserver.T_JSSE_Hierarchy.Org_Id);
                        observer.Dept_Id = Convert.ToString(dbObserver.T_JSSE_Hierarchy.Dept_Id);
                        observer.Section_Id = Convert.ToString(dbObserver.T_JSSE_Hierarchy.Sect_Id);
                        observer.User_ID = dbObserver.User_Id;
                        observer.UserRole = dbObserver.UserRole;
                        if (dbObserver.UserRole != "Supervisor")
                        {
                            oObservers.Add(observer);
                            if (!string.IsNullOrEmpty(selObservers))
                                selObservers = selObservers + "; " + observer.FullName;
                            else
                                selObservers = observer.FullName;
                        }
                        else
                        {
                            oSupervisors.Add(observer);
                            if (!string.IsNullOrEmpty(selSupervisors))
                                selSupervisors = selSupervisors + "; " + observer.FullName;
                            else
                                selSupervisors = observer.FullName;
                        }

                    }
                    if (oObservers.Count > 0)
                    {
                        if (string.IsNullOrEmpty(oObservers[0].UserRole))
                            oJsse.IsSupervsrOBSRSame = -1;
                        else
                            oJsse.IsSupervsrOBSRSame = 1;
                        if (oObservers[0].User_ID.ToLower() == "anonymous")
                            oJsse.IsOBSRAnonymous = true;
                        else
                            oJsse.IsOBSRAnonymous = false;
                        if (oSupervisors.Count > 0)
                        {
                            if (oObservers[0].Emp_Id != oSupervisors[0].Emp_Id)
                                oJsse.IsSupervsrOBSRSame = 0;
                            else
                                oJsse.IsSupervsrOBSRSame = 1;
                        }
                    }

                    oJsse.Observers = oObservers.ToArray();
                    oJsse.SelObservers = selObservers;
                    oJsse.Supervisors = oSupervisors.ToArray();
                    oJsse.SelSupervisors = selSupervisors;
                    //Observees
                    List<UserInfo> oObservees = new List<UserInfo>();
                    var selObservees = string.Empty;
                    foreach (var dbObservee in dbJSSE.T_JSSE_Observee.Where(j => j.Active == true))
                    {
                        UserInfo observee = new UserInfo();
                        observee.Emp_Id = Convert.ToInt64(dbObservee.Emp_ID);
                        observee.FirstName = dbObservee.FirstName;
                        observee.LastName = dbObservee.LastName;
                        observee.FullName = dbObservee.LastName + " , " + dbObservee.FirstName;
                        observee.MajorGroup_Id = Convert.ToString(dbObservee.T_JSSE_Hierarchy.MajorGroup_Id);
                        observee.Org_Id = Convert.ToString(dbObservee.T_JSSE_Hierarchy.Org_Id);
                        observee.Dept_Id = Convert.ToString(dbObservee.T_JSSE_Hierarchy.Dept_Id);
                        observee.Section_Id = Convert.ToString(dbObservee.T_JSSE_Hierarchy.Sect_Id);
                        observee.User_ID = dbObservee.User_Id;
                        //observee.UserTitle = "User Title";
                        oObservees.Add(observee);
                        if (!string.IsNullOrEmpty(selObservees))
                            selObservees = selObservees + "; " + observee.FullName;
                        else
                            selObservees = observee.FullName;
                    }
                    oJsse.Observees = oObservees.ToArray();
                    oJsse.SelObservees = selObservees;

                    oJsse.JSSE_ID = dbJSSE.JSSE_ID;
                    oJsse.Base64_JSSE_ID = dbJSSE.JSSE_ID.ToString();//Crypto.DecryptStringAES(Convert.ToString(dbJSSE.JSSE_ID), "JSSE");
                    oJsse.JobName = dbJSSE.JobName;
                    oJsse.JobDescription = dbJSSE.JobDescription;
                    oJsse.JSSEDate = dbJSSE.JSSEDate;
                    oJsse.JSSEStatus = dbJSSE.Status;
                    oJsse.Location = dbJSSE.Location;
                    var createdId = dbJSSE.CreatedBy != "Anonymous" && !dbJSSE.CreatedBy.StartsWith("CONED") ? "CONED\\" + dbJSSE.CreatedBy : dbJSSE.CreatedBy;
                    if (dbJSSE.CreatedBy != "Anonymous")
                    {
                        var createdUser = JSSESecurityManager.GetUserInfo(createdId);
                        if (createdUser != null)
                            oJsse.JSSECreator = createdUser.LAST_NAME + ", " + createdUser.FIRST_NAME;
                        else
                            oJsse.JSSECreator = dbJSSE.CreatedBy;
                    }
                    else
                        oJsse.JSSECreator = dbJSSE.CreatedBy;
                    oJsse.JSSEEnteredBy = dbJSSE.CreatedBy;
                    oJsse.Region_ID = dbJSSE.T_JSSE_Master_Region.Region_ID;
                    oJsse.Location = dbJSSE.Location;
                    oJsse.MajorGroup_Id = Convert.ToInt32(oJsse.Observees[0].MajorGroup_Id);
                    oJsse.Org_Id = Convert.ToInt32(oJsse.Observees[0].Org_Id);
                    oJsse.Dept_Id = !string.IsNullOrEmpty(oJsse.Observees[0].Dept_Id) ? Convert.ToInt32(oJsse.Observees[0].Dept_Id) : 0;
                    oJsse.Section_Id = !string.IsNullOrEmpty(oJsse.Observees[0].Section_Id) ? Convert.ToInt32(oJsse.Observees[0].Section_Id) : 0;
                    oJsse.Hierarchy_ID = dbJSSE.T_JSSE_Observee.FirstOrDefault(d => d.Active == true) != null ? dbJSSE.T_JSSE_Observee.FirstOrDefault(d => d.Active == true).T_JSSE_Hierarchy.Hierarchy_ID : 0;

                    //oJsse.majorgroups = GetMajorGroups().ToArray();
                    //oJsse.organizations = GetOrganizations(oJsse.MajorGroup_Id.ToString()).ToArray();
                    //oJsse.departments = GetDepartments(oJsse.Org_Id.ToString()).ToArray();
                    //oJsse.sections = GetSections(oJsse.Dept_Id.ToString()).ToArray();
                    List<JSSECategory> oSelCats = new List<JSSECategory>();


                    //Build the selected cats
                    foreach (var dbSelCat in dbJSSE.T_JSSE_Category.Where(j => j.Active == true))
                    {
                        JSSECategory oSelCat = new JSSECategory();
                        oSelCat.Category_ID = dbSelCat.T_JSSE_Master_Category.Category_ID;
                        if (dbSelCat.T_JSSE_Master_Rating != null)
                            oSelCat.RatingID = Convert.ToString(dbSelCat.T_JSSE_Master_Rating.Rating_ID);
                        else
                            oSelCat.RatingID = "0";
                        oSelCat.Comments = dbSelCat.T_JSSE_Comment.SingleOrDefault().Comment;
                        oSelCat.JSSE_ID = dbSelCat.T_JSSE_Main.JSSE_ID;
                        oSelCat.IsActive = Convert.ToBoolean(dbSelCat.Active);

                        //Build the selecrted behs
                        List<JSSEBehavior> oSelBehs = new List<JSSEBehavior>();
                        foreach (var dbSelBeh in dbSelCat.T_JSSE_Behavior.Where(j => j.Active == true))
                        {
                            JSSEBehavior oSelBeh = new JSSEBehavior();
                            oSelBeh.Behavior_ID = dbSelBeh.T_JSSE_Master_Behavior.Behavior_ID;
                            oSelBeh.BehaviorType_ID = dbSelBeh.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                            oSelBeh.Category_ID = dbSelBeh.T_JSSE_Category.JSSECategory_ID;
                            if (dbSelBeh.T_JSSE_Master_Rating != null)
                            {
                                oSelBeh.Rating_ID = dbSelBeh.T_JSSE_Master_Rating.Rating_ID;
                                oSelBeh.Rating = dbSelBeh.T_JSSE_Master_Rating.Rating;
                            }
                            else
                                oSelBeh.Rating_ID = 2; // if rating is blank for old records then rating is effective
                            oSelBeh.Comments = dbSelBeh.Comment;
                            oSelBeh.IsActive = Convert.ToBoolean(dbSelBeh.Active);
                            oSelBehs.Add(oSelBeh);
                        }
                        oSelCat.EntBehaviors = oSelBehs.ToArray();
                        oSelCats.Add(oSelCat);
                    }

                    //Can be fine-tuned later
                    List<JSSECategory> oAllCats = null;
                    if (oSelCats.Count > 0)
                        oAllCats = GetAllMasterCategories(oJsse.Org_Id);
                    else
                        oAllCats = GetAllActiveMasterCategories(oJsse.Org_Id);

                    foreach (var oSelCat in oSelCats)
                    {
                        foreach (var oCat in oAllCats)
                        {
                            if (oCat.Category_ID == oSelCat.Category_ID)
                            {
                                oCat.RatingID = oSelCat.RatingID;
                                oCat.Comments = oSelCat.Comments;
                                oCat.JSSE_ID = oSelCat.JSSE_ID;

                                foreach (var oBeh in oCat.EntBehaviors)
                                {
                                    foreach (var oSelBeh in oSelCat.EntBehaviors)
                                    {
                                        if (oBeh.Behavior_ID == oSelBeh.Behavior_ID)
                                        {
                                            oBeh.BehviorChecked = true;
                                            oBeh.Category_ID = oSelBeh.Category_ID;
                                            oBeh.Rating_ID = oSelBeh.Rating_ID;
                                            oBeh.Rating = oSelBeh.Rating;
                                            oBeh.Comments = oSelBeh.Comments;
                                        }
                                    }
                                }
                                foreach (var oBeh in oCat.OrgBehaviors)
                                {
                                    foreach (var oSelBeh in oSelCat.EntBehaviors)
                                    {
                                        if (oBeh.Behavior_ID == oSelBeh.Behavior_ID)
                                        {
                                            oBeh.BehviorChecked = true;
                                            oBeh.Category_ID = oSelBeh.Category_ID;
                                            oBeh.Rating_ID = oSelBeh.Rating_ID;
                                            oBeh.Rating = oSelBeh.Rating;
                                            oBeh.Comments = oSelBeh.Comments;
                                        }
                                    }
                                }
                            }
                            oCat.EntBehaviors = oCat.EntBehaviors.Where(eb => eb.IsActive == true || eb.Category_ID != null).ToArray();
                            oCat.OrgBehaviors = oCat.OrgBehaviors.Where(ob => ob.IsActive == true || ob.Category_ID != null).ToArray();
                        }
                    }
                    oJsse.Categories = oAllCats.Where(c => c.IsActive == true || c.JSSE_ID != null).ToArray();


                    //Get the attachments for the JSSE
                    List<JSSEAttachment> oAttachments = new List<JSSEAttachment>();
                    foreach (var dbAttachment in dbJSSE.T_JSSE_Attachments.Where(j => j.Active == true))
                    {
                        JSSEAttachment attachment = new JSSEAttachment();
                        attachment.Title = dbAttachment.Title;
                        attachment.Description = dbAttachment.Description;
                        attachment.FileName = dbAttachment.FileName;
                        attachment.Base64ImageString = CreateBase64Image(dbAttachment.Image);
                        oAttachments.Add(attachment);
                    }
                    oJsse.Attachments = oAttachments.ToArray();
                }
            }
            catch (Exception ex) { throw ex; }
            return oJsse;
        }

        private string CreateBase64Image(byte[] fileBytes)
        {
            using (MemoryStream ms = new MemoryStream(fileBytes))
            {
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public List<JSSECategory> GetAllMasterCategories(int? org_Id)
        {
            List<JSSECategory> oJsses = new List<JSSECategory>();
            try
            {
                var dbJsses = jsseRepo.GetCategoryBehaviors().ToList();
                var dbRatings = jsseRepo.GetRatings().ToList();
                foreach (var dbJSSE in dbJsses)
                {
                    JSSECategory oJsse = new JSSECategory();
                    oJsse.Category_ID = dbJSSE.Category_ID;
                    oJsse.Category = dbJSSE.Category;
                    oJsse.CategoryDesc = dbJSSE.CategoryDesc;
                    oJsse.SortOrder = dbJSSE.SortOrder;
                    oJsse.IsActive = Convert.ToBoolean(dbJSSE.Active);
                    // to validate comments required
                    if (dbJSSE.IsRequired == true)
                        oJsse.IsRequired = true;
                    else
                        oJsse.IsRequired = false;
                    List<JSSEBehavior> objMBehaviors = new List<JSSEBehavior>();
                    List<JSSERating> objRatings = new List<JSSERating>();
                    foreach (var dbCat in dbJSSE.T_JSSE_Master_CategoryBehavior_Map)
                    {
                        JSSEBehavior objMBehav = new JSSEBehavior();
                        objMBehav.Behavior_ID = dbCat.T_JSSE_Master_Behavior.Behavior_ID;
                        objMBehav.Behavior = dbCat.T_JSSE_Master_Behavior.Behavior;
                        objMBehav.SortOrder = dbCat.T_JSSE_Master_Behavior.SortOrder;
                        objMBehav.BehaviorDesc = dbCat.T_JSSE_Master_Behavior.BehaviorDesc;
                        objMBehav.BehaviorType_ID = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                        objMBehav.BehaviorType = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType;
                        objMBehav.Org_ID = dbCat.T_JSSE_Master_Behavior.Org_ID;
                        objMBehav.IsActive = Convert.ToBoolean(dbCat.Active);
                        objMBehaviors.Add(objMBehav);
                    }
                    objMBehaviors = objMBehaviors.OrderBy(o => o.SortOrder).ToList();
                    foreach (var dbRate in dbRatings)
                    {
                        JSSERating objRate = new JSSERating();
                        objRate.Rating_ID = dbRate.Rating_ID;
                        objRate.Rating = dbRate.Rating;
                        objRatings.Add(objRate);
                    }
                    int entBehavID = (int)BehaviorType.Enterprise;
                    int orgBehavID = (int)BehaviorType.Organization;
                    oJsse.EntBehaviors = objMBehaviors.Where(d => d.BehaviorType_ID == entBehavID).ToArray();
                    oJsse.OrgBehaviors = objMBehaviors.Where(d => d.BehaviorType_ID == orgBehavID && d.Org_ID == org_Id).ToArray();
                    oJsse.Ratings = objRatings.ToArray();
                    oJsse.IsActive = Convert.ToBoolean(dbJSSE.Active);
                    oJsses.Add(oJsse);
                }
                oJsses = oJsses.OrderBy(o => o.SortOrder).ToList();
            }

            catch
            { }
            return oJsses;
        }

        public List<JSSECategory> GetAllActiveMasterCategories(string userName)
        {
            List<JSSECategory> oJsses = new List<JSSECategory>();
            try
            {
                List<string> myOrgList =new List<string>();
                userName = string.IsNullOrWhiteSpace(userName) ? null : "Coned\\" + userName;
                var orgArrays = GetUserMajorGroups(userName, 2).Select(s => s.Organizations);
                foreach (var orgArray in orgArrays)
                {
                    foreach (var myOrg in orgArray)
                    {
                        if (myOrgList.IndexOf(myOrg.Org_Id) < 0)
                            myOrgList.Add(myOrg.Org_Id);
                    }
                }
                var dbJsses = jsseRepo.GetCategoryBehaviors().ToList();
                var dbRatings = jsseRepo.GetRatings().ToList();
                foreach (var dbJSSE in dbJsses)
                {
                    JSSECategory oJsse = new JSSECategory();
                    oJsse.Category_ID = dbJSSE.Category_ID;
                    oJsse.Category = dbJSSE.Category;
                    oJsse.CategoryDesc = dbJSSE.CategoryDesc;
                    oJsse.SortOrder = dbJSSE.SortOrder;
                    // to validate comments required
                    if (dbJSSE.IsRequired == true)
                        oJsse.IsRequired = true;
                    else
                        oJsse.IsRequired = false;
                    List<JSSEBehavior> objMBehaviors = new List<JSSEBehavior>();
                    List<JSSERating> objRatings = new List<JSSERating>();
                    foreach (var dbCat in dbJSSE.T_JSSE_Master_CategoryBehavior_Map)
                    {
                        JSSEBehavior objMBehav = new JSSEBehavior();
                        objMBehav.Behavior_ID = dbCat.T_JSSE_Master_Behavior.Behavior_ID;
                        objMBehav.Behavior = dbCat.T_JSSE_Master_Behavior.Behavior;
                        objMBehav.BehaviorDesc = dbCat.T_JSSE_Master_Behavior.BehaviorDesc;
                        objMBehav.SortOrder = dbCat.T_JSSE_Master_Behavior.SortOrder;
                        objMBehav.BehaviorType_ID = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                        objMBehav.BehaviorType = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType;
                        objMBehav.Org_ID = dbCat.T_JSSE_Master_Behavior.Org_ID;
                        objMBehav.IsActive = Convert.ToBoolean(dbCat.Active);
                        objMBehaviors.Add(objMBehav);
                    }
                    objMBehaviors = objMBehaviors.OrderBy(o => o.SortOrder).ToList();
                    foreach (var dbRate in dbRatings)
                    {
                        JSSERating objRate = new JSSERating();
                        objRate.Rating_ID = dbRate.Rating_ID;
                        objRate.Rating = dbRate.Rating;
                        objRatings.Add(objRate);
                    }
                    int entBehavID = (int)BehaviorType.Enterprise;
                    int orgBehavID = (int)BehaviorType.Organization;
                    oJsse.EntBehaviors = objMBehaviors.Where(d => d.BehaviorType_ID == entBehavID && d.IsActive == true).ToArray();
                    List<JSSEBehavior[]> orgBehaviors = new List<JSSEBehavior[]>();
                    foreach (string org in myOrgList)
                    {
                        int orgId = Int32.Parse(org);
                        var orgs = objMBehaviors.Where(d => d.BehaviorType_ID == orgBehavID && d.Org_ID == orgId && d.IsActive == true).ToArray();
                        orgBehaviors.Add(orgs);
                    }
                    oJsse.AllOrgBehaviors = orgBehaviors;
                    oJsse.Ratings = objRatings.ToArray();
                    oJsse.IsActive = Convert.ToBoolean(dbJSSE.Active);
                    //oJsse.Comments = "test";
                    oJsses.Add(oJsse);
                }
                oJsses = oJsses.Where(c => c.IsActive == true).OrderBy(x => x.SortOrder).ToList();
            }

            catch
            { }
            return oJsses;
        }

        public List<JSSECategory> GetAllActiveMasterCategories(int? org_Id)
        {
            List<JSSECategory> oJsses = new List<JSSECategory>();
            try
            {               
                var dbJsses = jsseRepo.GetCategoryBehaviors().ToList();
                var dbRatings = jsseRepo.GetRatings().ToList();
                foreach (var dbJSSE in dbJsses)
                {
                    JSSECategory oJsse = new JSSECategory();
                    oJsse.Category_ID = dbJSSE.Category_ID;
                    oJsse.Category = dbJSSE.Category;
                    oJsse.CategoryDesc = dbJSSE.CategoryDesc;
                    oJsse.SortOrder = dbJSSE.SortOrder;
                    // to validate comments required
                    if (dbJSSE.IsRequired == true)
                        oJsse.IsRequired = true;
                    else
                        oJsse.IsRequired = false;
                    List<JSSEBehavior> objMBehaviors = new List<JSSEBehavior>();
                    List<JSSERating> objRatings = new List<JSSERating>();
                    foreach (var dbCat in dbJSSE.T_JSSE_Master_CategoryBehavior_Map)
                    {
                        JSSEBehavior objMBehav = new JSSEBehavior();
                        objMBehav.Behavior_ID = dbCat.T_JSSE_Master_Behavior.Behavior_ID;
                        objMBehav.Behavior = dbCat.T_JSSE_Master_Behavior.Behavior;
                        objMBehav.BehaviorDesc = dbCat.T_JSSE_Master_Behavior.BehaviorDesc;
                        objMBehav.SortOrder = dbCat.T_JSSE_Master_Behavior.SortOrder;
                        objMBehav.BehaviorType_ID = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType_ID;
                        objMBehav.BehaviorType = dbCat.T_JSSE_Master_Behavior.T_JSSE_Master_BehaviorType.BehaviorType;
                        objMBehav.Org_ID = dbCat.T_JSSE_Master_Behavior.Org_ID;
                        objMBehav.IsActive = Convert.ToBoolean(dbCat.Active);
                        objMBehaviors.Add(objMBehav);
                    }
                    objMBehaviors = objMBehaviors.OrderBy(o => o.SortOrder).ToList();
                    foreach (var dbRate in dbRatings)
                    {
                        JSSERating objRate = new JSSERating();
                        objRate.Rating_ID = dbRate.Rating_ID;
                        objRate.Rating = dbRate.Rating;
                        objRatings.Add(objRate);
                    }
                    int entBehavID = (int)BehaviorType.Enterprise;
                    int orgBehavID = (int)BehaviorType.Organization;
                    oJsse.EntBehaviors = objMBehaviors.Where(d => d.BehaviorType_ID == entBehavID && d.IsActive == true).ToArray();
                    oJsse.OrgBehaviors = objMBehaviors.Where(d => d.BehaviorType_ID == orgBehavID && d.Org_ID == org_Id && d.IsActive == true).ToArray();
                    oJsse.Ratings = objRatings.ToArray();
                    oJsse.IsActive = Convert.ToBoolean(dbJSSE.Active);
                    //oJsse.Comments = "test";
                    oJsses.Add(oJsse);
                }
                oJsses = oJsses.Where(c => c.IsActive == true).OrderBy(x => x.SortOrder).ToList();
            }

            catch
            { }
            return oJsses;
        }

        public int AddModifyJSSE(JSSEMain jsse)
        {
            int result = 0;
            //add jsse, hierarchy, observee, observer
            var dbJsseMain = jsseRepo.AddModifyJsseMain(jsse).ToList()[0];
            jsse.JSSE_ID = dbJsseMain.JSSE_ID;
            jsse.Hierarchy_ID = dbJsseMain.Hierarchy_ID;

            //add categories, comments
            foreach (JSSECategory category in jsse.Categories)
            {
                result = Convert.ToInt16(jsseRepo.AddModifyJsseCategory(jsse, category));
            }

            return result;
        }
             

        public List<MajorGroupall> GetMajorGroups(string userId)
        {
            List<MajorGroupall> oMajorGroups = jsseRepo.GetMajorGroup(userId).ToList<MajorGroupall>();
            return oMajorGroups;
        }

        public List<Organization> GetOrganizations(string majorGroupId)
        {
            var orgs = jsseRepo.GetOrganizations(majorGroupId);
            return orgs.ToList();
        }

        public List<Department> GetDepartments(string orgId)
        {
            var depts = jsseRepo.GetDepartments(orgId);
            return depts.ToList();
        }

        public List<Section> GetSections(string deptId)
        {
            var sects = jsseRepo.GetSections(deptId);
            return sects.ToList();
        }

        public List<MajorGroup> GetMajorGroups()
        {
            //List<MajorGroup> oMajorGroups = new List<MajorGroup>();
            var mGroups = jsseRepo.GetMajorGroups();
            return mGroups.ToList();
        }

        public List<UserInfo> SearchOrgUsers(string firstName, string lastName, int org_Id)
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            var dbUsers = JSSESecurityManager.SearchOrgUsers(firstName, lastName, org_Id);
            foreach (var dbUser in dbUsers)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.CompanyId = dbUser.Company_Cd;
                userInfo.Emp_Id = dbUser.Emp_No;
                userInfo.FirstName = dbUser.FIRST_NAME;
                userInfo.LastName = dbUser.LAST_NAME;
                userInfo.FullName = dbUser.LAST_NAME + " , " + dbUser.FIRST_NAME;
                userInfo.Email = dbUser.EMAIL_ADDRESS_COMPANY;
                userInfo.User_ID = dbUser.PRIMARY_WINDOWS_NT_ACCOUNT;
                userInfo.Org_Id = dbUser.ORG_CD.ToString();
                userInfo.MajorGroup_Id = dbUser.VP_CD.ToString();
                userInfo.Dept_Id = dbUser.DEPT_CD.ToString();
                userInfo.Section_Id = dbUser.SECT_CD.ToString();
                userInfos.Add(userInfo);
            }
            return userInfos.Distinct().ToList();
        }

        public List<UserInfo> SearchUsers(string firstName, string lastName)
        {
            List<UserInfo> userInfos = new List<UserInfo>();
            var dbUsers = JSSESecurityManager.SearchUsers(firstName, lastName);
            foreach (var dbUser in dbUsers)
            {
                UserInfo userInfo = new UserInfo();
                userInfo.CompanyId = dbUser.Company_Cd;
                userInfo.Emp_Id = dbUser.Emp_No;
                userInfo.FirstName = dbUser.FIRST_NAME;
                userInfo.LastName = dbUser.LAST_NAME;
                userInfo.FullName = dbUser.LAST_NAME + " , " + dbUser.FIRST_NAME;
                userInfo.Email = dbUser.EMAIL_ADDRESS_COMPANY;
                userInfo.User_ID = dbUser.PRIMARY_WINDOWS_NT_ACCOUNT;
                userInfo.Org_Id = dbUser.ORG_CD.ToString();
                userInfo.MajorGroup_Id = dbUser.VP_CD.ToString();
                userInfo.Dept_Id = dbUser.DEPT_CD.ToString();
                userInfo.Section_Id = dbUser.SECT_CD.ToString();
                userInfos.Add(userInfo);
            }
            return userInfos.Distinct().ToList();
        }

        public List<Organization> GetUserOrgs(string userId, int groupType)
        {
            List<Organization> orgs = new List<Organization>();
            try
            {
                var dbOrgs = JSSESecurityManager.GetUserOrganizations(userId, groupType);
                foreach (var dbOrg in dbOrgs)
                {
                    if (!orgs.Any(o => o.Org_Id == dbOrg.org_cd))
                    {
                        Organization org = new Organization();
                        org.Org_Id = dbOrg.org_cd;
                        org.Org_Name = dbOrg.org_level_name;
                        org.MajorGroup_Id = dbOrg.vp_cd;
                        org.MajorGroup_Name = dbOrg.vp_level_name;
                        org.Departments = null;
                        orgs.Add(org);
                    }
                }
            }
            catch 
            {
                throw;
            }
            return orgs.Distinct().ToList();
        }
        public List<MajorGroup> GetUserMajorGroups(string userId, int groupType)
        {
            List<MajorGroup> mgs = new List<MajorGroup>();
            List<Organization> orgs = null;
            List<Department> depts = null;
            List<Section> sects = null;
            try
            {
                var dbMGs = JSSESecurityManager.GetUserOrganizations(userId, groupType);
                foreach (var dbMG in dbMGs)
                {
                    if (!mgs.Any(o => o.MajorGroup_Id == dbMG.vp_cd))
                    {
                        MajorGroup mg = new MajorGroup();
                        mg.MajorGroup_Id = dbMG.vp_cd;
                        mg.MajorGroup_Name = dbMG.vp_level_name;

                        var dbOrgs = dbMGs.Where(m => m.vp_cd == dbMG.vp_cd);
                        orgs = new List<Organization>();
                        foreach (var dbOrg in dbOrgs)
                        {
                            if (!orgs.Any(o => o.Org_Id == dbOrg.org_cd))
                            {
                                Organization org = new Organization();
                                org.Org_Id = dbOrg.org_cd;
                                org.Org_Name = dbOrg.org_level_name;

                                var dbDepts = dbOrgs.Where(m => m.org_cd == dbOrg.org_cd);
                                depts = new List<Department>();
                                foreach (var dbDept in dbDepts)
                                {
                                    if (!depts.Any(o => o.Dept_Id == dbDept.dept_cd))
                                    {
                                        Department dept = new Department();
                                        dept.Dept_Id = dbDept.dept_cd;
                                        dept.Dept_Name = dbDept.dept_level_name;

                                        var dbSects = dbDepts.Where(m => m.dept_cd == dbDept.dept_cd);
                                        sects = new List<Section>();
                                        foreach (var dbSect in dbSects)
                                        {
                                            if (!sects.Any(o => o.Section_Id == dbDept.sect_cd))
                                            {
                                                Section sect = new Section();
                                                sect.Section_Id = dbSect.sect_cd;
                                                sect.Section_Name = dbSect.sect_level_name;
                                                sects.Add(sect);
                                            }
                                        }
                                        dept.Sections = sects.ToArray();
                                        depts.Add(dept);
                                    }
                                }
                                org.Departments = depts.ToArray();
                                orgs.Add(org);
                            }
                        }
                        mg.Organizations = orgs.ToArray();
                        mgs.Add(mg);
                    }
                }
            }
            catch 
            {
                throw;
            }
            return mgs.Distinct().ToList();
        }
        public List<Organization> GetUserOrgsByPermission(string userId, int groupType, int permissionID)
        {
            List<Organization> orgs = new List<Organization>();
            try
            {
                var dbOrgs = JSSESecurityManager.GetUserOrgsByPermission(userId, groupType, permissionID);
                foreach (var dbOrg in dbOrgs)
                {
                    if (!orgs.Any(o => o.Org_Id == dbOrg.org_cd))
                    {
                        Organization org = new Organization();
                        org.Org_Id = dbOrg.org_cd;
                        org.Org_Name = dbOrg.org_level_name;
                        org.MajorGroup_Id = dbOrg.vp_cd;
                        org.MajorGroup_Name = dbOrg.vp_level_name;
                        org.Departments = null;
                        orgs.Add(org);
                    }
                }
            }
            catch 
            {
                throw;
            }
            return orgs.Distinct().ToList();
        }

        public List<Organization> GetObserveeOrgs(string userId, int groupType)
        {
            List<Organization> orgs = new List<Organization>();
            try
            {
                var dbOrgs = JSSESecurityManager.GetObserveeOrgs(userId, groupType);
                foreach (var dbOrg in dbOrgs)
                {
                    if (!orgs.Any(o => o.Org_Id == dbOrg.org_cd))
                    {
                        Organization org = new Organization();
                        org.Org_Id = dbOrg.org_cd;
                        org.Org_Name = dbOrg.org_level_name;
                        org.Departments = null;
                        orgs.Add(org);
                    }
                }
            }
            catch 
            {
                throw;
            }
            return orgs.Distinct().ToList();
        }

        public List<Organization> GetOrgName(string orgId)
        {
            List<Organization> orgs = new List<Organization>();
            try
            {
                var dbOrgs = JSSESecurityManager.GetOrgName(orgId);
                foreach (var dbOrg in dbOrgs)
                {
                    if (!orgs.Any(o => o.Org_Id == dbOrg.org_cd))
                    {
                        Organization org = new Organization();
                        org.Org_Id = dbOrg.org_cd;
                        org.Org_Name = dbOrg.org_level_name;
                        org.Departments = null;
                        orgs.Add(org);
                    }
                }
            }
            catch
            {
                throw ;
            }
            return orgs.Distinct().ToList();
        }

        public void LogErrortoDatabase(Exception ex, string createdBy, string function)
        {
            T_JSSE_Log log = new T_JSSE_Log();
            log.Message = ex.Message;
            log.CreatedBy = createdBy;
            log.ModifiedBy = createdBy;
            log.ModifiedDate = DateTime.Now;
            log.CreatedDate = DateTime.Now;
            log.Active = true;
            log.StackTrace = ex.StackTrace;
            log.AppURL = ex.TargetSite.Name;
            log.EventCategory = ex.Source;
            log.EventFunction = function;
            log.EventType = "Error";
            JSSELogManager.LogtoDatabase(log);
        }

        public int InsertUpdateJSSE(JSSEMain jsse)
        {
            try
            {
                return jsseRepo.InsertUpdateJSSE(jsse);
            }
            catch (Exception ex)
            {
                LogErrortoDatabase(ex, jsse.JSSEEnteredBy, "InsertUpdateJSSE");
                throw;
            }            
        }

        public int UpdateJSSETemp(JSSEMain jsse, byte[] image, string imageName)
        {
            T_JSSE_Main jsseModel = new T_JSSE_Main();
            jsseModel.ModifiedBy = jsse.JSSEEnteredBy.ToLower().IndexOf("coned") >=0 ? jsse.JSSEEnteredBy : "CONED\\" +jsse.JSSEEnteredBy;
            jsseModel.Location = jsse.Location;
            jsseModel.Status = jsse.JSSEStatus;
            jsseModel.Region_ID = jsse.Region_ID;
            jsseModel.JSSEDate = jsse.JSSEDate;
            jsseModel.JSSE_ID = jsse.JSSE_ID;
            return jsseRepo.UpdateJSSETemp(jsseModel, image, imageName);
        }

    }

    public enum BehaviorType
    {
        Enterprise = 1,
        Organization = 2
    }
}
