using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PERI.Prompt.EF;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogAttachment : ISampleData<EF.BlogAttachment>
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogAttachment(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.BlogAttachment args)
        {
            unitOfWork.BlogAttachmentRepository.Add(args);
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

        public Task Delete(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EF.BlogAttachment>> Find(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.BlogAttachment> Get(EF.BlogAttachment args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogAttachment>> Get(int[] ids)
        {
            var res = await unitOfWork.BlogAttachmentRepository.Entities.Where(x => ids.Contains(x.BlogId)).ToListAsync();

            return res;
        }
    }
}
