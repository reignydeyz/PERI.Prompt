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
    [Route("api/Section")]
    public class SectionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public SectionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("All")]
        public async Task<IActionResult> GetSections()
        {
            var template = (await new BLL.Template(unitOfWork).Find(new EF.Template())).Where(x => x.DateInactive == null).First();

            var obj = from r in (await new BLL.Section(unitOfWork).Find(new EF.Section { TemplateId = template.TemplateId }))
                      select new
                      {
                          r.SectionId,
                          r.Name
                      };
                        
            return Json(obj);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var obj = await new BLL.Section(unitOfWork).Get(new EF.Section { SectionId = id });

            return Json(new {
                sectionId = obj.SectionId,
                name = obj.Name,
                items = from r in obj.SectionItem
                        select new
                        {
                            r.Title,
                            r.Body,
                            Photo = r.SectionItemPhoto.FirstOrDefault() == null ? "" : r.SectionItemPhoto.First().Photo.Url,
                            Properties = from r1 in r.SectionItemProperty
                                         select new
                                         {
                                             r1.SectionProperty.Name,
                                             r1.Value
                                         }
                        }
            });
        }
    }
}