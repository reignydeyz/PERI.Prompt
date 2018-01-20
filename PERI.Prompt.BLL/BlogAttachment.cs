using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Prompt.EF;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogAttachment : ISampleData<EF.BlogAttachment>
    {
        EF.SampleDbContext context;

        public BlogAttachment(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogAttachment args)
        {
            context.BlogAttachment.Add(args);
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

        public Task Delete(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EF.BlogAttachment>> Find(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.BlogAttachment> Get(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogAttachment>> Get(int[] ids)
        {
            var res = await context.BlogAttachment.Where(x => ids.Contains(x.BlogId)).ToListAsync();

            return res;
        }
    }
}
