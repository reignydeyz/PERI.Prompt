using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PERI.Prompt.BLL
{
    public class BaseTemplateController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            using (var context = new EF.SampleDbContext())
            {
                var template = context.Template.First(x => x.DateInactive == null);

                ViewBag.LatestBlogs = context.Blog.Where(x => x.DateInactive == null).Take(5);

                ViewBag.Categories = from c in context.Category.Where(x => x.DateInactive == null)
                                     join bc in context.BlogCategory on c.CategoryId equals bc.CategoryId
                                     join b in context.Blog.Where(x => x.DateInactive == null) on bc.BlogId equals b.BlogId
                                     group c by new { c.CategoryId, c.Name } into g
                                     select new
                                     {
                                         g.Key.CategoryId,
                                         g.Key.Name,
                                         Blogs = g.Count()
                                     };

                ViewBag.Menus = context.Menu.Include(x => x.MenuItem).ThenInclude(x => x.ChildMenuItem).ToList();

                ViewBag.Tags = context.Tag.Include(x => x.BlogTag).ToList();

                ViewBag.Sections = context.Section
                    .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.SectionProperty).ThenInclude(x => x.SectionItemProperty)
                    .Where(x => x.TemplateId == template.TemplateId).ToList();

                base.OnActionExecuting(filterContext);
            }
        }
    }
}
