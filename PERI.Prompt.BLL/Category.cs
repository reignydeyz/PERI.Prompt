using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Category : ISampleData<EF.Category>
    {
        EF.SampleDbContext context;

        public Category(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            var res = context.Category.Where(x => ids.Contains(x.CategoryId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await context.SaveChangesAsync();
        }

        public async Task<int> Add(EF.Category args)
        {
            context.Category.Add(args);
            
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;

            await context.SaveChangesAsync();

            return args.CategoryId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = context.Category.Where(x => ids.Contains(x.CategoryId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Category.RemoveRange(context.Category.Where(x => ids.Contains(x.CategoryId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Category args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Category args)
        {
            var rec = context.Category.First(x => x.CategoryId == args.CategoryId);

            rec.Name = args.Name;
            rec.BlogSortOrderId = args.BlogSortOrderId;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.Category>> Find(EF.Category args)
        {
            var res = await (from c in context.Category
                             where c.Name.Contains(args.Name ?? string.Empty)
                             && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                             select c).ToListAsync();

            return res;
        }

        public async Task<EF.Category> Get(EF.Category args)
        {
            var rec = await (from c in context.Category
                             where c.Name == args.Name
                             select c).FirstOrDefaultAsync();
            return rec;
        }

        public async Task<Tuple<EF.Category, bool>> GetModel(int id)
        {
            var rec = await context.Category
                .FirstAsync(x => x.CategoryId == id);

            return new Tuple<EF.Category, bool>(rec, rec.DateInactive == null);
        }
    }
}
