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
    public class Menu : ISampleData<EF.Menu>
    {
        private readonly IUnitOfWork unitOfWork;

        public Menu(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Menu args)
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

        public Task Delete(EF.Menu args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Menu args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Menu>> Find(EF.Menu args)
        {
            var res = await(from c in unitOfWork.MenuRepository.Entities
                            .Include(x => x.MenuItem).ThenInclude(x => x.ChildMenuItem)
                            where c.Name.Contains(args.Name ?? string.Empty)
                            select c).ToListAsync();

            return res;
        }

        public async Task<EF.Menu> Get(EF.Menu args)
        {
            var rec = await unitOfWork.MenuRepository.Entities
                .Include(x => x.MenuItem).ThenInclude(x => x.ChildMenuItem)
                .FirstAsync(x => x.MenuId == args.MenuId);

            return rec;
        }
    }
}
