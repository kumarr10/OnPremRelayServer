using System.Web.Http;
using EHSApps.API.JSSE.Entity;
using EHSApps.API.JSSE.Services;

namespace EHSApps.WebAPI.JSSE.Controllers
{
    [RoutePrefix("api/jsse/category")]
    public class CategoryController : ApiController
    {
        #region "All GET Service Methods"

        [HttpGet]
        [Route("")]
        public MasterCategory[] GetCategories()
        {
            return CategoryService.GetCategories().ToArray();
        }

        [HttpPost]
        [Route("addupdate")]
        public int AddUpdateCategory([FromBody]MasterCategory category)
        {
            if (category.Category_ID <= 0)
                return CategoryService.AddCategory(category);
            else
                return CategoryService.UpdateCategory(category);
        }

        #endregion

        #region "All POST Service Methods"

        #endregion      
    }
}
