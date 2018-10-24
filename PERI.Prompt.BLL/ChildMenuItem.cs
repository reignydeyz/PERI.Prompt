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
    public class ChildMenuItem : ISampleData<EF.ChildMenuItem>
    {
        private IUnitOfWork unitOfWork;

        public ChildMenuItem(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.ChildMenuItem args)
        {
            await unitOfWork.ChildMenuItemRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

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
            unitOfWork.ChildMenuItemRepository.RemoveRange(unitOfWork.ChildMenuItemRepository.Entities.Where(x => ids.Contains(x.ChildMenuItemId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.ChildMenuItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.ChildMenuItem args)
        {
            var rec = await unitOfWork.ChildMenuItemRepository.Entities.FirstAsync(x => x.ChildMenuItemId == args.ChildMenuItemId);
            rec.Label = args.Label;
            rec.Url = args.Url;
            rec.Order = args.Order;
            await unitOfWork.CommitAsync();
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
