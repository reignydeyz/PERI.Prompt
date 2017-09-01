using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;

namespace PERI.Prompt.BLL
{
    public class BlogCategory : ISampleData<EF.BlogCategory>
    {
        EF.SampleDbContext context;

        public BlogCategory(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogCategory args)
        {
            await context.BlogCategory.AddAsync(args);
            await context.SaveChangesAsync();

            return args.BlogId;
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

        public Task Delete(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EF.BlogCategory>> Find(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.BlogCategory> Get(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }
    }
}
