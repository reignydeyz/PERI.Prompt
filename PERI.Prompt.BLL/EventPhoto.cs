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
    public class EventPhoto : ISampleData<EF.EventPhoto>
    {
        EF.SampleDbContext context;

        public EventPhoto(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.EventPhoto args)
        {
            context.EventPhoto.Add(args);
            await context.SaveChangesAsync();
            return args.EventId;
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

        public Task Delete(EF.EventPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.EventPhoto args)
        {
            var ep = context.EventPhoto.FirstOrDefault(x => x.EventId == args.EventId && x.PhotoId == args.PhotoId);

            if (ep == null)
            {
                await Add(args);
            }
        }

        public async Task<IEnumerable<EF.EventPhoto>> Find(EF.EventPhoto args)
        {
            var res = await context.EventPhoto.Where(x => x.EventId == args.EventId).ToListAsync();

            return res;
        }

        public Task<EF.EventPhoto> Get(EF.EventPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.EventPhoto>> Get(int[] ids)
        {
            var res = await context.EventPhoto.Where(x => ids.Contains(x.EventId)).ToListAsync();

            return res;
        }
    }
}
