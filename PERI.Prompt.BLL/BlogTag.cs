using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogTag : ISampleData<EF.BlogTag>
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogTag(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogTag args)
        {
            unitOfWork.BlogTagRepository.Add(args);
            await unitOfWork.CommitAsync();
            return args.TagId;
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

        public Task Delete(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.BlogTag args)
        {
            var bt = unitOfWork.BlogTagRepository.Entities.FirstOrDefault(x => x.BlogId == args.BlogId && x.TagId == args.TagId);

            if (bt == null)
            {
                await Add(args);
            }
        }

        public Task<IEnumerable<EF.BlogTag>> Find(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.BlogTag> Get(EF.BlogTag args)
        {
            throw new NotImplementedException();
        }

        public async Task Clean(string[] tags)
        {
            var tagsToBeRemoved = from res in unitOfWork.BlogTagRepository.Entities
                            .Include(x => x.Tag)
                                    where !tags.Contains(res.Tag.Name)
                                    select res;

            foreach (var rec in tagsToBeRemoved)
                unitOfWork.BlogTagRepository.Remove(rec);

            await unitOfWork.CommitAsync();
        }
    }
}
