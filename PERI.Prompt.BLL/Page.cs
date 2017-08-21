using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    public class Page : ISampleData<EF.Page>
    {
        EF.SampleDbContext context;

        public Page(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            var res = context.Page.Where(x => ids.Contains(x.PageId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await context.SaveChangesAsync();
        }

        public async Task<int> Add(EF.Page args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            context.Page.Add(args);
            await context.SaveChangesAsync();
            return args.PageId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = context.Page.Where(x => ids.Contains(x.PageId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Page.RemoveRange(context.Page.Where(x => ids.Contains(x.PageId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Page args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Page args)
        {
            var rec = context.Page.First(x => x.PageId == args.PageId);
            rec.Title = args.Title;
            rec.Permalink = args.Permalink;
            rec.Content = args.Content;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.Page>> Find(EF.Page args)
        {
            var res = await (from c in context.Page
                                where c.Permalink.Contains(args.Content ?? string.Empty)
                                && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                                select c).ToListAsync();

            return res;
        }

        public Task<EF.Page> Get(EF.Page args)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<EF.Page, bool>> GetModel(int id)
        {
            var rec = await context.Page
                .Include(x => x.PagePhoto).ThenInclude(x => x.Photo)
                .FirstAsync(x => x.PageId == id);
                
            return new Tuple<EF.Page, bool>(rec, rec.DateInactive == null);
        }
    }
}
