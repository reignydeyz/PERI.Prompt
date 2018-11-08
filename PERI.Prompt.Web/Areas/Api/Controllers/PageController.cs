using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

namespace PERI.Prompt.Web.Areas.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Page")]
    public class PageController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public PageController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> All()
        {
            var res = from r in (await new BLL.Page(unitOfWork).Find(new EF.Page()))
                      select new
                      {
                          r.PageId,
                          r.Title,
                          r.Content,
                          r.CreatedBy,
                          r.DateCreated,
                          r.DateModified,
                          r.ModifiedBy,
                          PhotoUrl = r.PagePhoto.FirstOrDefault() == null ? "" : r.PagePhoto.First().Photo.Url
                      };

            return Json(res);
        }

        [HttpGet("[action]")]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var obj = await new BLL.Page(unitOfWork).Get(new EF.Page { PageId = id });

            if (obj != null)
            {
                dynamic obj1 = new ExpandoObject();
                obj1.pageId = obj.PageId;
                obj1.title = obj.Title;
                obj1.content = obj.Content;
                obj1.createdBy = obj.CreatedBy;
                obj1.dateCreated = obj.DateCreated;
                obj1.modifiedBy = obj.ModifiedBy;
                obj1.dateModified = obj.DateModified;
                obj1.photoUrl = obj.PagePhoto.FirstOrDefault() == null ? "" : obj.PagePhoto.First().Photo.Url;
                return Json(obj1);
            }
            else
                return StatusCode(403);
        }
    }
}