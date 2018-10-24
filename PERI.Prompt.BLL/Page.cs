using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Page : ISampleData<EF.Page>
    {
        private readonly IUnitOfWork unitOfWork;

        public Page(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.PageRepository.Entities.Where(x => ids.Contains(x.PageId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.Page args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            unitOfWork.PageRepository.Add(args);
            await unitOfWork.CommitAsync();
            return args.PageId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.PageRepository.Entities.Where(x => ids.Contains(x.PageId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.PageRepository.RemoveRange(unitOfWork.PageRepository.Entities.Where(x => ids.Contains(x.PageId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Page args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Page args)
        {
            var rec = await unitOfWork.PageRepository.Entities.FirstAsync(x => x.PageId == args.PageId);
            rec.Title = args.Title;
            rec.Permalink = args.Permalink;
            rec.Content = args.Content;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.Page>> Find(EF.Page args)
        {
            var res = await (from c in unitOfWork.PageRepository.Entities
                                where c.Permalink.Contains(args.Content ?? string.Empty)
                                && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                                select c).ToListAsync();

            return res;
        }

        public async Task<EF.Page> Get(EF.Page args)
        {
            return await unitOfWork.PageRepository.Entities
                .Include(x => x.PagePhoto).ThenInclude(x => x.Photo)
                .FirstOrDefaultAsync(x => x.Permalink == args.Permalink);
        }

        public async Task<Tuple<EF.Page, bool>> GetModel(int id)
        {
            var rec = await unitOfWork.PageRepository.Entities
                .Include(x => x.PagePhoto).ThenInclude(x => x.Photo)
                .FirstAsync(x => x.PageId == id);
                
            return new Tuple<EF.Page, bool>(rec, rec.DateInactive == null);
        }
    }
}
