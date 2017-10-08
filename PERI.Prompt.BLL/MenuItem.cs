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
    public class MenuItem : ISampleData<EF.MenuItem>
    {
        EF.SampleDbContext context;

        public MenuItem(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.MenuItem args)
        {
            await context.MenuItem.AddAsync(args);
            await context.SaveChangesAsync();

            return args.MenuItemId;
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
            context.MenuItem.RemoveRange(context.MenuItem.Where(x => ids.Contains(x.MenuItemId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.MenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.MenuItem args)
        {
            var rec = await  context.MenuItem.FirstAsync(x => x.MenuItemId == args.MenuItemId);
            rec.Label = args.Label;
            rec.Url = args.Url;
            rec.Order = args.Order;
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<EF.MenuItem>> Find(EF.MenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task<EF.MenuItem> Get(EF.MenuItem args)
        {
            return await context.MenuItem
                .Include(x => x.Menu)
                .Include(x => x.ChildMenuItem)
                .FirstOrDefaultAsync(x => x.MenuItemId == args.MenuItemId);
        }
    }
}
