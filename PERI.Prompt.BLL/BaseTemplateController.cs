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

                ViewBag.Menus = context.Menu.Include(x => x.MenuItem).ThenInclude(x => x.ChildMenuItem).ToList();
                ViewBag.Sections = context.Section
                    .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.SectionProperty).ThenInclude(x => x.SectionItemProperty)
                    .Where(x => x.TemplateId == template.TemplateId).ToList();

                base.OnActionExecuting(filterContext);
            }
        }
    }
}
