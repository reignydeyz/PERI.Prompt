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
        private readonly IUnitOfWork unitOfWork;

        public Setting(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
            var res = await unitOfWork.SettingRepository.Entities.FirstAsync(x => 
            x.Key == args.Key
            && x.Group == args.Group);
            return res;
        }

        public async Task<IEnumerable<EF.Setting>> Find(EF.Setting args)
        {
            var res = await unitOfWork.SettingRepository.Entities.Where(x => x.Group.Contains(args.Group ?? x.Group)).ToListAsync();
            return res;
        }
    }
}
