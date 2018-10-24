using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BaseTemplateController : BaseController
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var unitOfWork = new UnitOfWork(new EF.SampleDbContext());
            
            var template = unitOfWork.TemplateRepository.Entities.First(x => x.DateInactive == null);

            var now = DateTime.Now;

            ViewBag.LatestBlogs = unitOfWork.BlogRepository.Entities
                .Where(x => x.DateInactive == null && x.DatePublished <= now)
                .OrderByDescending(x => x.DatePublished).Take(5)
                .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo).ToList();

            ViewBag.Categories = (from c in unitOfWork.CategoryRepository.Entities.Where(x => x.DateInactive == null)
                                    join bc in unitOfWork.BlogCategoryRepository.Entities on c.CategoryId equals bc.CategoryId
                                    join b in unitOfWork.BlogRepository.Entities.Where(x => x.DateInactive == null && x.DatePublished <= now) on bc.BlogId equals b.BlogId
                                    group c by new { c.CategoryId, c.Name } into g
                                    select new CategoryBlogs
                                    {
                                        CategoryId = g.Key.CategoryId,
                                        CategoryName = g.Key.Name,
                                        BlogCount = g.Count()
                                    }).ToList();

            ViewBag.Events = unitOfWork.EventRepository.Entities.Where(x => x.DateInactive == null && x.Time > DateTime.Now)
                                .Include(x => x.EventPhoto).ThenInclude(x => x.Photo).OrderBy(x => x.Time).ToList();

            ViewBag.Menus = unitOfWork.MenuRepository.Entities.Include(x => x.MenuItem).ThenInclude(x => x.ChildMenuItem).ToList();

            ViewBag.Tags = unitOfWork.TagRepository.Entities.Include(x => x.BlogTag)
                .Where(x => x.BlogTag.Count() > 0)
                .OrderByDescending(x => x.BlogTag.Count())
                .ToList();

            ViewBag.Sections = unitOfWork.SectionRepository.Entities
                .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.SectionProperty).ThenInclude(x => x.SectionItemProperty)
                .Where(x => x.TemplateId == template.TemplateId).ToList();

            base.OnActionExecuting(filterContext);
        }
    }
}
