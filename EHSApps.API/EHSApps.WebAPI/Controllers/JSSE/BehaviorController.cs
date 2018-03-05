using System.Web.Http;
using EHSApps.API.JSSE.Entity;
using EHSApps.API.JSSE.Services;
using System.Net.Http.Formatting;

namespace EHSApps.WebAPI.JSSE.Controllers
{
    [RoutePrefix("api/jsse/behavior")]
    public class BehaviorController : ApiController
    {
        #region "All GET Service Methods"

        [HttpPost]
        [Route("addupdate")]
        public int AddUpdateBehavior([FromBody]MasterBehavior behavior)
        {
            if (behavior.Behavior_ID <= 0)
                return BehaviorService.AddBehavior(behavior);
            else
                return BehaviorService.UpdateBehavior(behavior);
        }

        [HttpGet]
        [Route("")]
        public MasterBehavior[] GetBehaviors([FromBody]BehaviorFilter data)
        {
            //int categoryId; int behaviorTypeId; int majorGroupId; int orgId;

            //int.TryParse(data.Get("categoryId"), out categoryId);
            //int.TryParse(data.Get("behaviorTypeId"), out behaviorTypeId);
            //int.TryParse(data.Get("majorGroupId"), out majorGroupId);
            //int.TryParse(data.Get("orgId"), out orgId);
            return BehaviorService.GetBehaviors(data.CategoryId, data.BehaviorTypeId, data.MajorGroupId, data.OrgId).ToArray();
        }

        #endregion

        #region "All POST Service Methods"

        #endregion

        //#region Simple Test
        //[HttpGet]
        //[Route("{action}")]
        //public string GetOne()
        //{
        //    return "One";
        //}

        //[Route("{action}")]
        //public string GetTwo()
        //{
        //    return "Two";
        //}
        //#endregion
    }
}
