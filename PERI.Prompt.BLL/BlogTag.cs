using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogTag : ISampleData<EF.BlogTag>
    {
        EF.SampleDbContext context;

        public BlogTag(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogTag args)
        {
            context.BlogTag.Add(args);
            await context.SaveChangesAsync();
            return args.TagId;
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

        public Task Delete(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.BlogTag args)
        {
            var bt = context.BlogTag.FirstOrDefault(x => x.BlogId == args.BlogId && x.TagId == args.TagId);

            if (bt == null)
            {
                await Add(args);
            }
        }

        public Task<IEnumerable<EF.BlogTag>> Find(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.BlogTag> Get(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public async Task Clean(string[] tags)
        {
            var tagsToBeRemoved = from res in context.BlogTag
                            .Include(x => x.Tag)
                                    where !tags.Contains(res.Tag.Name)
                                    select res;

            foreach (var rec in tagsToBeRemoved)
                context.Remove(rec);

            await context.SaveChangesAsync();
        }
    }
}
