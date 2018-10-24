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
    public class SectionItem : ISampleData<EF.SectionItem>
    {
        private readonly IUnitOfWork unitOfWork;

        public SectionItem(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.SectionItem args)
        {
            unitOfWork.SectionItemRepository.Add(args);
            await unitOfWork.CommitAsync();

            return args.SectionItemId;
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
            var res = unitOfWork.SectionItemRepository.Entities.Where(x => ids.Contains(x.SectionItemId));
            unitOfWork.SectionItemRepository.RemoveRange(res);
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.SectionItem args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.SectionItem args)
        {
            var rec = await unitOfWork.SectionItemRepository.Entities.FirstAsync(x => x.SectionItemId == args.SectionItemId);
            rec.Title = args.Title ?? rec.Title;
            rec.Body = args.Body ?? args.Body;
            rec.Order = args.Order;
            rec.ModifiedBy = args.ModifiedBy ?? rec.ModifiedBy;
            rec.DateModified = DateTime.Now;

            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.SectionItem>> Find(EF.SectionItem args)
        {
            var res = await unitOfWork.SectionItemRepository.Entities
                .Include(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.Section)
                .Include(x => x.SectionItemProperty).ThenInclude(x => x.SectionProperty)
                .Where(x => x.Title.Contains(args.Title ?? string.Empty))
                .ToListAsync();

            return res;
        }

        public async Task<EF.SectionItem> Get(EF.SectionItem args)
        {
            var rec = await unitOfWork.SectionItemRepository.Entities
                .Include(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.Section)
                .Include(x => x.SectionItemProperty).ThenInclude(x => x.SectionProperty)
                .FirstOrDefaultAsync(x => x.SectionItemId == args.SectionItemId);

            return rec;
        }
    }
}
