using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PERI.Prompt.BLL
{
    public class Template : ISampleData<EF.Template>
    {
        EF.SampleDbContext context;

        public Template(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Template args)
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

        public Task Delete(EF.Template args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Template args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Template>> Find(EF.Template args)
        {
            var res = await context.Template.Where(x => x.Name.Contains(args.Name ?? x.Name)).ToListAsync();
            return res;
        }

        public async Task<EF.Template> Get(EF.Template args)
        {
            var res = await context.Template.FirstOrDefaultAsync(x =>
                x.Name == (args.Name ?? x.Name)
                && x.TemplateId == (args.TemplateId == 0 ? x.TemplateId : args.TemplateId)
            );

            return res;
        }
    }
}
