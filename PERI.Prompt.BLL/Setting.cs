using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Setting : ISampleData<EF.Setting>
    {
        EF.SampleDbContext context;

        public Setting(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Setting args)
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

        public Task Delete(EF.Setting args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Setting args)
        {
            using (var context = new EF.SampleDbContext())
            {
                var row = context.Setting.First(x => x.SettingId == args.SettingId);
                row.Value = args.Value;
                await context.SaveChangesAsync();
            }
        }

        public async Task<EF.Setting> Get(EF.Setting args)
        {
            var res = await context.Setting.FirstAsync(x => 
            x.Key == args.Key
            && x.Group == args.Group);
            return res;
        }

        public async Task<IEnumerable<EF.Setting>> Find(EF.Setting args)
        {
            var res = await context.Setting.Where(x => x.Group.Contains(args.Group ?? x.Group)).ToListAsync();
            return res;
        }
    }
}
