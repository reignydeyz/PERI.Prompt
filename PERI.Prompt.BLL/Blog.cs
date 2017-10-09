using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Blog : ISampleData<EF.Blog>
    {
        EF.SampleDbContext context;

        public Blog(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            var res = context.Blog.Where(x => ids.Contains(x.BlogId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await context.SaveChangesAsync();
        }

        public async Task<int> Add(EF.Blog args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            context.Blog.Add(args);
            await context.SaveChangesAsync();
            return args.BlogId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = context.Blog.Where(x => ids.Contains(x.BlogId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Blog.RemoveRange(context.Blog.Where(x => ids.Contains(x.BlogId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Blog args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Blog args)
        {
            var rec = context.Blog.First(x => x.BlogId == args.BlogId);
            rec.Title = args.Title;
            rec.Body = args.Body;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.Blog>> Find(EF.Blog args)
        {
            var res = await (from c in context.Blog
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                             where c.Title.Contains(args.Title ?? string.Empty)
                             && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                             select c).ToListAsync();

            return res;
        }

        /// <summary>
        /// Gets the Blogs within the Category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <param name="blogSortOrderId">Blog Sort Order Id</param>
        /// <returns>List of Blogs</returns>
        public async Task<IEnumerable<EF.Blog>> FindByCategoryId(int id)
        {
            var blogSortOrderId = (await context.Category.FirstAsync(x => x.CategoryId == id)).BlogSortOrderId;

            var res = await (from b in context.Blog
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                    join bc in context.BlogCategory on b.BlogId equals bc.BlogId
                    join c in context.Category on bc.CategoryId equals c.CategoryId
                    where c.CategoryId == id select b).ToListAsync();

            switch (blogSortOrderId)
            {
                // Ascending by DateCreated
                case 1:
                    return res.OrderBy(x => x.DateCreated).ToList();
                // Descending by DateCreated
                case 2:
                    return res.OrderByDescending(x => x.DateCreated).ToList();
                // Ascending by DatePublished
                case 3:
                    return res.OrderBy(x => x.DatePublished).ToList();
                // Descending by DatePublished
                default:
                case 4:
                    return res.OrderByDescending(x => x.DatePublished).ToList();
                // Ascending by Title
                case 5:
                    return res.OrderBy(x => x.Title).ToList();
                // Descending by Title
                case 6:
                    return res.OrderByDescending(x => x.Title).ToList();
            }
        }

        public async Task<EF.Blog> Get(EF.Blog args)
        {
            var rec = await context.Blog
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.BlogId == args.BlogId);

            return rec;
        }

        public Tuple<EF.Blog, string, bool, Dictionary<string, bool>> GetModel(int id)
        {
            var rec = context.Blog
            .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
            .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
            .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
            .First(x => x.BlogId == id);

            var csv = String.Join(",", rec.BlogTag.Select(x => x.Tag.Name));

            var dict = new Dictionary<string, bool>();
            var categories = from c in context.Category
                             join bc in rec.BlogCategory on c.CategoryId equals bc.CategoryId into jointable
                             from z in jointable.DefaultIfEmpty()
                             select new
                             {
                                 Key = c.Name,
                                 Value = z != null
                             };

            return new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(rec, csv, rec.DateInactive == null, categories.ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
