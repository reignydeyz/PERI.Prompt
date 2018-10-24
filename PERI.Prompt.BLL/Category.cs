using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Category : ISampleData<EF.Category>
    {
        private readonly IUnitOfWork unitOfWork;

        public Category(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.CategoryRepository.Entities.Where(x => ids.Contains(x.CategoryId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.Category args)
        {
            unitOfWork.CategoryRepository.Add(args);
            
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;

            await unitOfWork.CommitAsync();

            return args.CategoryId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.CategoryRepository.Entities.Where(x => ids.Contains(x.CategoryId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.CategoryRepository.RemoveRange(unitOfWork.CategoryRepository.Entities.Where(x => ids.Contains(x.CategoryId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Category args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Category args)
        {
            var rec = unitOfWork.CategoryRepository.Entities.First(x => x.CategoryId == args.CategoryId);

            rec.Name = args.Name;
            rec.BlogSortOrderId = args.BlogSortOrderId;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.Category>> Find(EF.Category args)
        {
            var res = await (from c in unitOfWork.CategoryRepository.Entities
                             where c.Name.Contains(args.Name ?? string.Empty)
                             && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                             select c).ToListAsync();

            return res;
        }

        public async Task<EF.Category> Get(EF.Category args)
        {
            var rec = await (from c in unitOfWork.CategoryRepository.Entities
                             where c.Name == args.Name
                             select c).FirstOrDefaultAsync();
            return rec;
        }

        public async Task<Tuple<EF.Category, bool>> GetModel(int id)
        {
            var rec = await unitOfWork.CategoryRepository.Entities
                .FirstAsync(x => x.CategoryId == id);

            return new Tuple<EF.Category, bool>(rec, rec.DateInactive == null);
        }
    }
}
