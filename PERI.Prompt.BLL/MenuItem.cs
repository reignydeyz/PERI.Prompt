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
        private readonly IUnitOfWork unitOfWork;

        public MenuItem(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.MenuItem args)
        {
            await unitOfWork.MenuItemRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

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
            unitOfWork.MenuItemRepository.RemoveRange(unitOfWork.MenuItemRepository.Entities.Where(x => ids.Contains(x.MenuItemId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.MenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.MenuItem args)
        {
            var rec = await  unitOfWork.MenuItemRepository.Entities.FirstAsync(x => x.MenuItemId == args.MenuItemId);
            rec.Label = args.Label;
            rec.Url = args.Url;
            rec.Order = args.Order;
            await unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<EF.MenuItem>> Find(EF.MenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task<EF.MenuItem> Get(EF.MenuItem args)
        {
            return await unitOfWork.MenuItemRepository.Entities
                .Include(x => x.Menu)
                .Include(x => x.ChildMenuItem)
                .FirstOrDefaultAsync(x => x.MenuItemId == args.MenuItemId);
        }
    }
}
