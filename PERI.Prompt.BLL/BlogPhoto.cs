using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogPhoto : ISampleData<EF.BlogPhoto>
    {
        EF.SampleDbContext context;

        public BlogPhoto(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogPhoto args)
        {
            context.BlogPhoto.Add(args);
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

        public Task Delete(EF.BlogPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.BlogPhoto args)
        {
            var bp = context.BlogPhoto.FirstOrDefault(x => x.BlogId == args.BlogId && x.PhotoId == args.PhotoId);

            if (bp == null)
            {
                await Add(args);
            }  
        }

        public async Task<IEnumerable<EF.BlogPhoto>> Find(EF.BlogPhoto args)
        {
            var res = await context.BlogPhoto.Where(x => x.BlogId == args.BlogId || args.PhotoId == args.PhotoId).ToListAsync();

            return res;
        }

        public Task<EF.BlogPhoto> Get(EF.BlogPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogPhoto>> Get(int[] ids)
        {
            var res = await context.BlogPhoto.Where(x => ids.Contains(x.BlogId)).ToListAsync();

            return res;
        }
    }
}
