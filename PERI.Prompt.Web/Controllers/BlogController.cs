﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PERI.Prompt.BLL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PERI.Prompt.Web.Controllers
{
    public class BlogController : BLL.BaseTemplateController
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

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
            var obj = await new BLL.Blog(unitOfWork).Get(new EF.Blog { BlogId = blogId });

            ViewData["Title"] = obj.Title;

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
            ViewData["Title"] = categoryName;

            var category = await new BLL.Category(unitOfWork).Get(new EF.Category { Name = categoryName });

            if (category == null)
                return StatusCode(404);
            else if (category.DateInactive != null)
                return StatusCode(403);

            var blogs = (await new BLL.Blog(unitOfWork).FindByCategoryId(category.CategoryId)).Where(x => x.DateInactive == null);

            var page = Convert.ToInt16(Request.Query["page"]);
            var pager = new Core.Pager(blogs.Count(), page == 0 ? 1 : page);

            blogs = blogs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize);

            return View(new Tuple<EF.Category, Core.Pager, List<EF.Blog>>(category, pager, blogs.ToList()));
        }
                
        public async Task<IActionResult> Preview()
        {
            var title = Request.Cookies["preview_blog_title"];
            var body = Request.Cookies["preview_blog_body"];

            var obj = new EF.Blog
            {
                Title = title,
                Body = body
            };

            if (Request.Cookies["preview_blog_photoId"] != "")
            {
                var id = Convert.ToInt32(Request.Cookies["preview_blog_photoId"]);
                var photo = await new BLL.Photo(unitOfWork).Get(new EF.Photo { PhotoId = id });

                obj.BlogPhoto.Add(new EF.BlogPhoto {
                    Photo = photo
                });
            }

            return View(obj);
        }
    }
}
