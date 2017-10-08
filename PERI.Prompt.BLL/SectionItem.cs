using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class SectionItem : ISampleData<EF.SectionItem>
    {
        EF.SampleDbContext context;

        public SectionItem(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.SectionItem args)
        {
            context.SectionItem.Add(args);
            await context.SaveChangesAsync();

            return args.SectionItemId;
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            var res = context.SectionItem.Where(x => ids.Contains(x.SectionItemId));
            context.SectionItem.RemoveRange(res);
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.SectionItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.SectionItem args)
        {
            var rec = await context.SectionItem.FirstAsync(x => x.SectionItemId == args.SectionItemId);
            rec.Title = args.Title ?? rec.Title;
            rec.Body = args.Body ?? args.Body;
            rec.Order = args.Order;
            rec.ModifiedBy = args.ModifiedBy ?? rec.ModifiedBy;
            rec.DateModified = DateTime.Now;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.SectionItem>> Find(EF.SectionItem args)
        {
            var res = await context.SectionItem
                .Include(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.Section)
                .Include(x => x.SectionItemProperty).ThenInclude(x => x.SectionProperty)
                .Where(x => x.Title.Contains(args.Title ?? string.Empty))
                .ToListAsync();

            return res;
        }

        public async Task<EF.SectionItem> Get(EF.SectionItem args)
        {
            var rec = await context.SectionItem
                .Include(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.Section)
                .Include(x => x.SectionItemProperty).ThenInclude(x => x.SectionProperty)
                .FirstOrDefaultAsync(x => x.SectionItemId == args.SectionItemId);

            return rec;
        }
    }
}
