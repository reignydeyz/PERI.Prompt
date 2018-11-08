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
    [Route("api/Blog")]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("[action]")]
        [Route("Category/{categoryName}")]
        public async Task<IActionResult> Category(string categoryName)
        {
            var category = await new BLL.Category(unitOfWork).Get(new EF.Category { Name = categoryName });

            if (category == null)
                return StatusCode(404);
            else if (category.DateInactive != null)
                return StatusCode(403);

            var res = from r in (await new BLL.Blog(unitOfWork).FindByCategoryId(category.CategoryId)).Where(x => x.DateInactive == null)
                      select new
                      {
                          r.BlogId,
                          r.Title,
                          r.Body,
                          r.DatePublished,
                          r.CreatedBy,
                          r.DateCreated,
                          r.ModifiedBy,
                          r.DateModified,
                          PhotoUrl = r.BlogPhoto.FirstOrDefault() == null ? "" : r.BlogPhoto.First().Photo.Url
                      };

            var page = Convert.ToInt16(Request.Query["page"]);
            var pager = new Core.Pager(res.Count(), page == 0 ? 1 : page);

            dynamic obj1 = new ExpandoObject();
            obj1.blogs = res.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList();
            obj1.pager = pager;

            return Json(obj1);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var obj = await new BLL.Blog(unitOfWork).Get(new EF.Blog { BlogId = id });

            if (obj != null && obj.DatePublished <= DateTime.Now)
            {
                dynamic obj1 = new ExpandoObject();
                obj1.blogId = obj.BlogId;
                obj1.title = obj.Title;
                obj1.body = obj.Body;
                obj1.datePublished = obj.DatePublished;
                obj1.createdBy = obj.CreatedBy;
                obj1.dateCreated = obj.DateCreated;
                obj1.modifiedBy = obj.ModifiedBy;
                obj1.dateModified = obj.DateModified;
                obj1.photoUrl = obj.BlogPhoto.FirstOrDefault() == null ? "" : obj.BlogPhoto.First().Photo.Url;
                return Json(obj1);
            }
            else
                return StatusCode(403);
        }
    }
}