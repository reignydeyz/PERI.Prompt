using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class SectionProperty : ISampleData<EF.SectionProperty>
    {
        EF.SampleDbContext context;

        public SectionProperty(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.SectionProperty args)
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

        public Task Delete(EF.SectionProperty args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.SectionProperty args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.SectionProperty>> Find(EF.SectionProperty args)
        {
            var res = await (from r in context.SectionProperty
                        where r.SectionId == (args.SectionId == 0 ? r.SectionId : args.SectionId)
                        && r.Name.Contains(args.Name ?? string.Empty)
                        select r).ToListAsync();

            return res;
        }

        public Task<EF.SectionProperty> Get(EF.SectionProperty args)
        {
            throw new NotImplementedException();
        }
    }
}
