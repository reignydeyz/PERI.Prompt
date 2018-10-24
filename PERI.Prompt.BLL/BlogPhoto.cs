using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogPhoto : ISampleData<EF.BlogPhoto>
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogPhoto(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogPhoto args)
        {
            unitOfWork.BlogPhotoRepository.Add(args);
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

        public Task Delete(EF.BlogPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.BlogPhoto args)
        {
            var bp = unitOfWork.BlogPhotoRepository.Entities.FirstOrDefault(x => x.BlogId == args.BlogId && x.PhotoId == args.PhotoId);

            if (bp == null)
            {
                await Add(args);
            }  
        }

        public async Task<IEnumerable<EF.BlogPhoto>> Find(EF.BlogPhoto args)
        {
            var res = await unitOfWork.BlogPhotoRepository.Entities.Where(x => x.BlogId == args.BlogId || args.PhotoId == args.PhotoId).ToListAsync();

            return res;
        }

        public Task<EF.BlogPhoto> Get(EF.BlogPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogPhoto>> Get(int[] ids)
        {
            var res = await unitOfWork.BlogPhotoRepository.Entities.Where(x => ids.Contains(x.BlogId)).ToListAsync();

            return res;
        }
    }
}
