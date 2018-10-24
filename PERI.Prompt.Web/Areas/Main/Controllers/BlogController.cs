using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using PERI.Prompt.BLL;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly EF.SampleDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHostingEnvironment _environment;
        public BlogController(IUnitOfWork unitOfWork, IHostingEnvironment environment, EF.SampleDbContext context)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            _environment = environment;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Blogs";

            ViewBag.Data = await new BLL.Blog(unitOfWork).Find(new EF.Blog());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EF.Blog args)
        {
            ViewData["Title"] = "Blogs";
            ViewBag.Data = await new BLL.Blog(unitOfWork).Find(args);
            return View();
        }

        public async Task<IActionResult> New()
        {            
            ViewData["Title"] = "Blog/New";
            
            var dict = new Dictionary<string, bool>();
            var categories = await new BLL.Category(unitOfWork).Find(new EF.Category());
            foreach (var c in categories)
                dict.Add(c.Name, false);

            var obj = new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(new EF.Blog { DatePublished = DateTime.Now }, string.Empty, true, dict);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([Bind(Prefix = "Item1")] EF.Blog model, [Bind(Prefix = "Item2")] string tags, [Bind(Prefix = "Item3")] bool isactive, [Bind(Prefix = "Item4")] Dictionary<string, bool> categories, IFormCollection files)
        {
            ViewData["Title"] = "Blog/New";

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    model.VisibilityId = 1;
                    model.CreatedBy = User.Identity.Name;

                    var bblog = new BLL.Blog(unitOfWork);

                    // Add Blog
                    if (!isactive)
                        model.DateInactive = DateTime.Now;
                    else
                        model.DateInactive = null;

                    var id = await bblog.Add(model);

                    // Add BlogCategory
                    foreach (var category in categories.Where(x => x.Value == true))
                    {
                        var c = await new BLL.Category(unitOfWork).Get(new EF.Category { Name = category.Key });
                        await new BLL.BlogCategory(unitOfWork).Add(new EF.BlogCategory { BlogId = id, CategoryId = c.CategoryId });
                    }

                    foreach (var file in files.Files)
                    {
                        if (file.Name == "photo")
                        {
                            // Add Photo
                            IFormFile uploadedImage = file;
                            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
                            {
                                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                                var bp = new EF.BlogPhoto();
                                bp.BlogId = id;
                                bp.PhotoId = pid;
                                await new BLL.BlogPhoto(unitOfWork).Add(bp);
                            }
                        }
                        else if (file.Name == "attachments")
                        {
                            // Add Attachment
                            if (file.Length > 0)
                            {
                                var aid = await new BLL.Attachment(unitOfWork).Add(_environment, file);

                                var ba = new EF.BlogAttachment();
                                ba.BlogId = id;
                                ba.AttachmentId = aid;
                                await new BLL.BlogAttachment(unitOfWork).Add(ba);
                            }
                        }
                    }

                    // Add Tag
                    if (tags != null)
                    {
                        foreach (var tag in tags.Split(','))
                        {
                            var tid = await new BLL.Tag(unitOfWork).Add(new EF.Tag { Name = tag });
                            await new BLL.BlogTag(unitOfWork).Add(new EF.BlogTag { BlogId = id, TagId = tid });
                        }
                    }

                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    TempData["notice"] = "Oops! Something went wrong.";

                    return View(new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(model, tags, isactive, categories));
                }
            }

            return Redirect("~/Main/Blog");
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Title"] = "Blog/Edit";

            var obj = await (new BLL.Blog(unitOfWork).GetModel(id));
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind(Prefix = "Item1")] EF.Blog model, [Bind(Prefix = "Item2")] string tags, [Bind(Prefix = "Item3")] bool isactive, [Bind(Prefix = "Item4")] Dictionary<string, bool> categories, IFormCollection files)
        {
            ViewData["Title"] = "Blog/Edit";

            using (var txn = context.Database.BeginTransaction())
            {
                try
                {
                    model.ModifiedBy = User.Identity.Name;

                    // Update Category
                    // Delete all
                    var bblogcategory = new BLL.BlogCategory(unitOfWork);
                    var cats = await bblogcategory.Find(new EF.BlogCategory { BlogId = model.BlogId });
                    await bblogcategory.Delete(cats.ToList());
                    // Add
                    foreach (var category in categories.Where(x => x.Value == true))
                    {
                        var c = await new BLL.Category(unitOfWork).Get(new EF.Category { Name = category.Key });
                        await new BLL.BlogCategory(unitOfWork).Add(new EF.BlogCategory { BlogId = model.BlogId, CategoryId = c.CategoryId });
                    }

                    // Update Blog
                    if (!isactive)
                        model.DateInactive = DateTime.Now;
                    else
                        model.DateInactive = null;

                    await new BLL.Blog(unitOfWork).Edit(model);

                    foreach (var file in files.Files)
                    {
                        if (file.Name == "photo")
                        {
                            // Update Photo
                            IFormFile uploadedImage = file;
                            if (uploadedImage != null && uploadedImage.ContentType.ToLower().StartsWith("image/"))
                            {
                                var bblogphoto = new BLL.BlogPhoto(unitOfWork);
                                var blogphotos = await bblogphoto.Find(new EF.BlogPhoto { BlogId = model.BlogId });
                                if (blogphotos.Count() > 0)
                                    await new BLL.Photo(unitOfWork).Delete(blogphotos.First().PhotoId, _environment);

                                var pid = await new BLL.Photo(unitOfWork).Add(_environment, file);

                                var bp = new EF.BlogPhoto();
                                bp.BlogId = model.BlogId;
                                bp.PhotoId = pid;
                                await bblogphoto.Edit(bp);
                            }
                        }
                        else if (file.Name == "attachments")
                        {
                            // Add Attachment
                            if (file.Length > 0)
                            {
                                var aid = await new BLL.Attachment(unitOfWork).Add(_environment, file);

                                var ba = new EF.BlogAttachment();
                                ba.BlogId = model.BlogId;
                                ba.AttachmentId = aid;
                                await new BLL.BlogAttachment(unitOfWork).Add(ba);
                            }
                        }
                    }

                    // Update Tag
                    if (tags != null)
                    {
                        foreach (var tag in tags.Split(','))
                        {
                            var tid = await new BLL.Tag(unitOfWork).Add(new EF.Tag { Name = tag });
                            await new BLL.BlogTag(unitOfWork).Edit(new EF.BlogTag { BlogId = model.BlogId, TagId = tid });
                        }
                    }

                    // Remove unwanted Tag
                    string[] tagsArr;
                    if (tags == null)
                        tagsArr = new string[] { string.Empty };
                    else
                        tagsArr = tags.Split(',');

                    await new BLL.BlogTag(unitOfWork).Clean(tagsArr);

                    txn.Commit();
                }
                catch (Exception ex)
                {
                    txn.Rollback();

                    logger.Error(ex);

                    TempData["notice"] = "Oops! Something went wrong.";

                    return View(new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(model, tags, isactive, categories));
                }
            }            

            return Redirect("~/Main/Blog");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int[] ids)
        {
            var bblog = new BLL.Blog(unitOfWork);

            // Delete photos
            var blogphotos = await new BLL.BlogPhoto(unitOfWork).Get(ids);
            if (blogphotos.Count() > 0)
                await new BLL.Photo(unitOfWork).Delete(blogphotos.Select(x => x.PhotoId).ToArray(), _environment);

            // Delete attachments
            var blogattachments = await new BLL.BlogAttachment(unitOfWork).Get(ids);
            if (blogattachments.Count() > 0)
                await new BLL.Attachment(unitOfWork).Delete(blogattachments.Select(x => x.AttachmentId).ToArray(), _environment);

            await bblog.Delete(ids);            

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Activate([FromBody] int[] ids)
        {
            var bblog = new BLL.Blog(unitOfWork);

            await bblog.Activate(ids);

            return Json("Success!");
        }

        [HttpPost]
        public async Task<IActionResult> Deactivate([FromBody] int[] ids)
        {
            var bblog = new BLL.Blog(unitOfWork);

            await bblog.Deactivate(ids);

            return Json("Success!");
        }
        
        [HttpPost]
        public async Task<IActionResult> Preview(EF.Blog blog, IFormCollection files)
        {
            var bllPhoto = new BLL.Photo(unitOfWork);
            var bllBlogPhoto = new BLL.BlogPhoto(unitOfWork);

            if (!String.IsNullOrEmpty(Request.Cookies["preview_blog_photoId"]))
            {
                var id = Convert.ToInt32(Request.Cookies["preview_blog_photoId"]);

                // Delete photo if not in BlogPhotos
                if ((await bllBlogPhoto.Find(new EF.BlogPhoto { PhotoId = id })).Count() <= 0)
                    await bllPhoto.Delete(id, _environment);
            }

            // Clear cookies
            Response.Cookies.Delete("preview_blog_title");
            Response.Cookies.Delete("preview_blog_body");
            Response.Cookies.Delete("preview_blog_photoId");

            // Add cookies
            Response.Cookies.Append("preview_blog_title", blog.Title);
            Response.Cookies.Append("preview_blog_body", blog.Body);

            foreach (var file in files.Files)
            {
                if (file.Name == "photo")
                {
                    int? photoId = null;
                    if (file != null && file.Length > 0)
                        photoId = await bllPhoto.Add(_environment, file);
                    else
                    {
                        if (blog.BlogId != 0)
                        {
                            var bllBlog = new BLL.Blog(unitOfWork);
                            var blo = await bllBlog.Get(new EF.Blog { BlogId = blog.BlogId });

                            if (blo.BlogPhoto.Count() > 0)
                                photoId = blo.BlogPhoto.First().PhotoId;
                        }
                    }

                    Response.Cookies.Append("preview_blog_photoId", photoId.ToString());
                }
            }

            return Json("Success!");
        }

        [Route("Main/Blog/DeleteBlogAttachment/{blogId:int}/{attachmentId:int}")]
        public async Task<IActionResult> DeleteBlogAttachment(int blogId, int attachmentId)
        {
            await new BLL.Attachment(unitOfWork).Delete(attachmentId, _environment);

            return new EmptyResult();
        }
    }
}
