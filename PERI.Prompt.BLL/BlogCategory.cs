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
    public class BlogCategory : ISampleData<EF.BlogCategory>
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogCategory(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogCategory args)
        {
            await unitOfWork.BlogCategoryRepository.AddAsync(args);
            await unitOfWork.CommitAsync();

            return args.BlogId;
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

        public Task Delete(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(List<EF.BlogCategory> args)
        {
            unitOfWork.BlogCategoryRepository.RemoveRange(args);
            await unitOfWork.CommitAsync();
        }

        public Task Edit(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogCategory>> Find(EF.BlogCategory args)
        {
            return await unitOfWork.BlogCategoryRepository.Entities.Where(x => x.BlogId == args.BlogId).ToListAsync();
        }

        public Task<EF.BlogCategory> Get(EF.BlogCategory args)
        {
            throw new NotImplementedException();
        }
    }
}
