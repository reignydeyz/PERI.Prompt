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
    public class SectionItemPhoto : ISampleData<EF.SectionItemPhoto>
    {
        private readonly IUnitOfWork unitOfWork;

        public SectionItemPhoto(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.SectionItemPhoto args)
        {
            unitOfWork.SectionItemPhotoRepository.Add(args);
            await unitOfWork.CommitAsync();
            return 0;
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

        public Task Delete(EF.SectionItemPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.SectionItemPhoto args)
        {
            var sip = unitOfWork.SectionItemPhotoRepository.Entities.FirstOrDefault(x => x.SectionItemId == args.SectionItemId && x.PhotoId == args.PhotoId);

            if (sip == null)
            {
                await Add(args);
            }
        }

        public async Task<IEnumerable<EF.SectionItemPhoto>> Find(EF.SectionItemPhoto args)
        {
            var res = await unitOfWork.SectionItemPhotoRepository.Entities.Where(x => x.SectionItemId == args.SectionItemId).ToListAsync();

            return res;
        }

        public Task<EF.SectionItemPhoto> Get(EF.SectionItemPhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.SectionItemPhoto>> Get(int[] ids)
        {
            var res = await unitOfWork.SectionItemPhotoRepository.Entities.Where(x => ids.Contains(x.SectionItemId)).ToListAsync();

            return res;
        }
    }
}
