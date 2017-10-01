using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    public class Category : ISampleData<EF.Category>
    {
        EF.SampleDbContext context;

        public Category(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
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

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Category args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Category args)
        {
            throw new NotImplementedException();
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
