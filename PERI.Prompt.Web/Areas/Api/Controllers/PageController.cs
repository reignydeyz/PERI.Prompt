using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

namespace PERI.Prompt.Web.Areas.Api.Controllers
{
    [EnableCors("MyPolicy")]
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
                          PhotoUrl = r.PagePhoto.FirstOrDefault() == null ? "" : Request.Scheme + "://" + Request.Host.Value + "/" + r.PagePhoto.First().Photo.Url
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
                return Json(new {
                    pageId = obj.PageId,
                    title = obj.Title,
                    content = obj.Content,
                    createdBy = obj.CreatedBy,
                    dateCreated = obj.DateCreated,
                    modifiedBy = obj.ModifiedBy,
                    dateModified = obj.DateModified,
                    photoUrl = obj.PagePhoto.FirstOrDefault() == null ? "" : obj.PagePhoto.First().Photo.Url
                });
            }
            else
                return StatusCode(403);
        }
    }
}