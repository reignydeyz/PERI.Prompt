using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Areas.Main.Controllers
{
    [Area("Main")]
    [Authorize(Roles = "Admin,Blogger")]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class BlogController : BLL.BaseController
    {
        private readonly IHostingEnvironment _environment;
        public BlogController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Blogs";

            var context = new EF.SampleDbContext();

            ViewBag.Data = await new BLL.Blog(context).Find(new EF.Blog());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Blog args)
        {
            ViewData["Title"] = "Blogs";
            var context = new EF.SampleDbContext();
            ViewBag.Data = await new BLL.Blog(context).Find(args);
            return View();
        }

        public async Task<IActionResult> New()
        {            
            ViewData["Title"] = "Blog/New";
            
            var context = new EF.SampleDbContext();
            var dict = new Dictionary<string, bool>();
            var categories = await new BLL.Category(context).Find(new EF.Category());
            foreach (var c in categories)
                dict.Add(c.Name, false);

            var obj = new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(new EF.Blog { DatePublished = DateTime.Now }, string.Empty, true, dict);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Blog model, [Bind(Prefix = "Item2")] string tags, [Bind(Prefix = "Item3")] bool isactive, [Bind(Prefix = "Item4")] Dictionary<string, bool> categories, IFormFile file)
        {
            ViewData["Title"] = "Blog/New";

            var context = new EF.SampleDbContext();

            model.CreatedBy = User.Identity.Name;

            var bblog = new BLL.Blog(context);

            // Add Blog
            if (!isactive)
                model.DateInactive = DateTime.Now;
            else
                model.DateInactive = null;

            var id = await bblog.Add(model);

            // Add BlogCategory
            foreach (var category in categories.Where(x => x.Value == true))
            {
                var c = await new BLL.Category(context).Get(new EF.Category { Name = category.Key });
                await new BLL.BlogCategory(context).Add(new EF.BlogCategory { BlogId = id, CategoryId = c.CategoryId });
            }

            // Add Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var pid = await new BLL.Photo(context).Add(_environment, file);

                var bp = new EF.BlogPhoto();
                bp.BlogId = id;
                bp.PhotoId = pid;
                await new BLL.BlogPhoto(context).Add(bp);
            }

            // Add Tag
            if (tags != null)
            {
                foreach (var tag in tags.Split(','))
                {
                    var tid = await new BLL.Tag(context).Add(new EF.Tag { Name = tag });
                    await new BLL.BlogTag(context).Add(new EF.BlogTag { BlogId = id, TagId = tid });
                }
            }

            return Redirect("~/Main/Blog");
        }

        public IActionResult Edit(int id)
        {
            ViewData["Title"] = "Blog/Edit";

            var context = new EF.SampleDbContext();

            var obj = new BLL.Blog(context).GetModel(id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.Blog model, [Bind(Prefix = "Item2")] string tags, [Bind(Prefix = "Item3")] bool isactive, [Bind(Prefix = "Item4")] Dictionary<string, bool> categories, IFormFile file)
        {
            ViewData["Title"] = "Blog/Edit";

            var context = new EF.SampleDbContext();

            model.ModifiedBy = User.Identity.Name;

            // Update Category
            // Delete all
            var bblogcategory = new BLL.BlogCategory(context);
            var cats = await bblogcategory.Find(new EF.BlogCategory { BlogId = model.BlogId });
            await bblogcategory.Delete(cats.ToList());
            // Add
            foreach (var category in categories.Where(x => x.Value == true))
            {
                var c = await new BLL.Category(context).Get(new EF.Category { Name = category.Key });
                await new BLL.BlogCategory(context).Add(new EF.BlogCategory { BlogId = model.BlogId, CategoryId = c.CategoryId });
            }

            // Update Blog
            if (!isactive)
                model.DateInactive = DateTime.Now;
            else
                model.DateInactive = null;

            await new BLL.Blog(context).Edit(model);

            // Update Photo
            IFormFile uploadedImage = file;
            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
            {
                var bblogphoto = new BLL.BlogPhoto(context);
                var blogphotos = await bblogphoto.Find(new EF.BlogPhoto { BlogId = model.BlogId });
                if (blogphotos.Count() > 0)
                    await new BLL.Photo(context).Delete(blogphotos.First().PhotoId, _environment);

                var pid = await new BLL.Photo(context).Add(_environment, file);

                var bp = new EF.BlogPhoto();
                bp.BlogId = model.BlogId;
                bp.PhotoId = pid;
                await bblogphoto.Edit(bp);
            }

            // Update Tag
            if (tags != null)
            {
                foreach (var tag in tags.Split(','))
                {
                    var tid = await new BLL.Tag(context).Add(new EF.Tag { Name = tag });
                    await new BLL.BlogTag(context).Edit(new EF.BlogTag { BlogId = model.BlogId, TagId = tid });
                }
            }

            // Remove unwanted Tag
            string[] tagsArr;
            if (tags == null)
                tagsArr = new string[] { string.Empty };
            else
                tagsArr = tags.Split(',');

            await new BLL.BlogTag(context).Clean(tagsArr);

            return Redirect("~/Main/Blog");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var bblog = new BLL.Blog(context);

            // Delete photos
            var blogphotos = await new BLL.BlogPhoto(context).Get(ids);
            if (blogphotos.Count() > 0)
                await new BLL.Photo(context).Delete(blogphotos.Select(x => x.PhotoId).ToArray(), _environment);

            await bblog.Delete(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var bblog = new BLL.Blog(context);

            await bblog.Activate(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var context = new EF.SampleDbContext();

            var bblog = new BLL.Blog(context);

            await bblog.Deactivate(ids);

            return Json("Success!");
        }
    }
}
