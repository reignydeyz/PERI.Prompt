using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class BlogController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the whole detail of the Category including its Blogs
        /// </summary>
        /// <param name="id">Id represents the name of the Category</param>
        /// <returns></returns>
        [Route("Category/{id}")]
        public async Task<IActionResult> Category(string id)
        {
            var context = new EF.SampleDbContext();

            var category = await new BLL.Category(context).Get(new EF.Category { Name = id });

            // Will only get 100 blogs per Category request
            return View(category.BlogCategory.Take(100));
        }
    }
}
