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
        [Route("Category/{categoryName}/{blogId:int}")]
        public IActionResult Index(string categoryName, int blogId)
        {
            var context = new EF.SampleDbContext();

            var obj = new BLL.Blog(context).GetModel(blogId);
            return View(obj);
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

            // Will only get 100 blogs per Category request
            return View(category.BlogCategory.Take(100));
        }
    }
}
