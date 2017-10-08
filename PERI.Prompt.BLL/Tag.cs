using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using System.Linq;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Tag : ISampleData<EF.Tag>
    {
        EF.SampleDbContext context;

        public Tag(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.Tag args)
        {
            var rec = context.Tag.FirstOrDefault(x => x.Name == args.Name);

            if (rec == null)
            {                    
                context.Tag.Add(args);
                await context.SaveChangesAsync();
                return args.TagId;
            }
            else
                return rec.TagId;
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

        public Task Delete(EF.Tag args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Tag args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EF.Tag>> Find(EF.Tag args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.Tag> Get(EF.Tag args)
        {
            throw new NotImplementedException();
        }
    }
}
