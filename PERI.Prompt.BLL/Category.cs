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

        public Task<int> Add(EF.Category args)
        {
            throw new NotImplementedException();
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
                      .Include(x => x.BlogCategory).ThenInclude(x => x.Blog)
                      .ThenInclude(x => x.BlogPhoto).ThenInclude(x => x.Photo)

                      .Include(x => x.BlogCategory).ThenInclude(x => x.Blog)
                      .ThenInclude(x => x.BlogTag).ThenInclude(x => x.Tag)
                             where c.Name == args.Name
                             select c).FirstAsync();
            return rec;
        }
    }
}
