using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
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
                        where c.Title.Contains(args.Title ?? string.Empty)
                        && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                        select c).ToListAsync();

            return res;
        }

        public Task<EF.Blog> Get(EF.Blog args)
        {
            throw new NotImplementedException();
        }

        public Tuple<EF.Blog, string, bool> GetModel(int id)
        {
            var rec = context.Blog
            .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
            .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
            .First(x => x.BlogId == id);

            var csv = String.Join(",", rec.BlogTag.Select(x => x.Tag.Name));

            return new Tuple<EF.Blog, string, bool>(rec, csv, rec.DateInactive == null);
        }
    }
}
