using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class SectionItemProperty : ISampleData<EF.SectionItemProperty>
    {
        EF.SampleDbContext context;

        public SectionItemProperty(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.SectionItemProperty args)
        {
            context.SectionItemProperty.Add(args);
            await context.SaveChangesAsync();
            return 0;
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

        public async Task Delete(EF.SectionItemProperty args)
        {
            var rec = await context.SectionItemProperty.FirstAsync(x => x.SectionPropertyId == args.SectionPropertyId && x.SectionItemId == args.SectionItemId);
            context.Remove(rec);
            await context.SaveChangesAsync();
        }

        public async Task Edit(EF.SectionItemProperty args)
        {
            var rec = await context.SectionItemProperty.FirstAsync(x => x.SectionItemId == args.SectionItemId
            && x.SectionPropertyId == args.SectionPropertyId);

            rec.Value = args.Value;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.SectionItemProperty>> Find(EF.SectionItemProperty args)
        {
            var res = await (from r in context.SectionItemProperty
                    .Include(x => x.SectionItem)
                    .Include(x => x.SectionProperty)
                        where r.SectionItem.SectionId == (args.SectionItem.SectionId == 0 ? r.SectionItem.SectionId : args.SectionItem.SectionId)
                        && r.SectionProperty.Name.Contains((args.SectionProperty ?? new EF.SectionProperty()).Name ?? string.Empty)
                        select r).ToListAsync();

            return res;
        }

        public async Task<EF.SectionItemProperty> Get(EF.SectionItemProperty args)
        {
            return await context.SectionItemProperty.FirstOrDefaultAsync(x => x.SectionItemId == args.SectionItemId
            && x.SectionPropertyId == args.SectionPropertyId);
        }
    }
}
