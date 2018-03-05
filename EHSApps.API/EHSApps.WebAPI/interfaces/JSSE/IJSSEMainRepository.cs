using ConEdison.EHSApps.WebAPI.Models;
using EHSApps.API.JSSE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace EHSApps.WebAPI.JSSE.Interfaces
{    
    public interface IJSSEMainRepository
    {
        IEnumerable<T_JSSE_Master_Region> GetRegions();

        IEnumerable<vw_JSSE_GetAllJSSEs> GetUserOrgJSSEs(int? org_Id, string user_Id, DateTime? fromDate, DateTime? toDate, bool isOwner);

        IEnumerable<T_JSSE_Master_Category> GetCategoryBehaviors();

        int UpdateJSSETemp(T_JSSE_Main jsse, byte[] image, string imgName);

        int InsertUpdateJSSE(JSSEMain jsse);

        IEnumerable<T_JSSE_Main> GetJSSE(string jsseId);

        IEnumerable<T_JSSE_Main> GetJSSE(int jsseId);

        IEnumerable<T_JSSE_Master_Rating> GetRatings();

        IEnumerable<MajorGroupall> GetMajorGroup(string userId);

        IEnumerable<MajorGroup> GetMajorGroups();

        IEnumerable<Organization> GetOrganizations(string mGroupId);

        IEnumerable<Department> GetDepartments(string orgId);

        IEnumerable<Section> GetSections(string deptId);

        int AddModifyJsseCategory(JSSEMain jsse, JSSECategory category);

        IEnumerable<JSSEMain> AddModifyJsseMain(JSSEMain jsse);



    }
}
