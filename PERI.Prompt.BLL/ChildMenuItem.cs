using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Prompt.BLL
{
    public class ChildMenuItem : ISampleData<EF.ChildMenuItem>
    {
        EF.SampleDbContext context;

        public ChildMenuItem(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.ChildMenuItem args)
        {
            await context.ChildMenuItem.AddAsync(args);
            await context.SaveChangesAsync();

            return args.ChildMenuItemId;
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.ChildMenuItem.RemoveRange(context.ChildMenuItem.Where(x => ids.Contains(x.ChildMenuItemId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.ChildMenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.ChildMenuItem args)
        {
            var rec = await context.ChildMenuItem.FirstAsync(x => x.ChildMenuItemId == args.ChildMenuItemId);
            rec.Label = args.Label;
            rec.Url = args.Url;
            rec.Order = args.Order;
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<EF.ChildMenuItem>> Find(EF.ChildMenuItem args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.ChildMenuItem> Get(EF.ChildMenuItem args)
        {
            throw new NotImplementedException();
        }
    }
}
