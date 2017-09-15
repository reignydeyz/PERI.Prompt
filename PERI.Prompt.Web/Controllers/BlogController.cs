using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class BlogController : BLL.BaseTemplateController
    {
        // GET: /<controller>/
        /// <summary>
        /// Views the whole content of the Blog
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        [Route("Category/{categoryName}/Blog/{blogId:int}")]
        public async Task<IActionResult> Index(string categoryName, int blogId)
        {
            var context = new EF.SampleDbContext();

            var obj = await new BLL.Blog(context).Get(new EF.Blog { BlogId = blogId });

            if (obj != null && obj.DatePublished <= DateTime.Now)
                return View(obj);
            else
                return StatusCode(403);
        }

        /// <summary>
        /// Gets the whole detail of the Category including its Blogs
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [Route("Category/{categoryName}")]
        public async Task<IActionResult> Category(string categoryName)
        {
            var context = new EF.SampleDbContext();

            var category = await new BLL.Category(context).Get(new EF.Category { Name = categoryName });
            var blogs = await new BLL.Blog(context).FindByCategoryId(category.CategoryId);

            return View(new Tuple<EF.Category, List<EF.Blog>>(category, blogs.ToList()));
        }
    }
}
