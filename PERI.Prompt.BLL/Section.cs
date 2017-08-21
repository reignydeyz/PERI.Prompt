using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Prompt.BLL
{
    public class Section : ISampleData<EF.Section>
    {
        EF.SampleDbContext context;

        public Section(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Section args)
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

        public Task Delete(EF.Section args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Section args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Section>> Find(EF.Section args)
        {
            var res = await context.Section
                .Where(x => x.TemplateId == args.TemplateId
                && x.Name.Contains(args.Name ?? string.Empty))
                .Include(x => x.Template)
                .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemProperty).ToListAsync();

            return res;
        }

        public async Task<EF.Section> Get(EF.Section args)
        {
            var rec = await context.Section
                .Include(x => x.Template)
                .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemProperty)
                .FirstOrDefaultAsync(x => x.SectionId == args.SectionId);

            return rec;
        }
    }
}
