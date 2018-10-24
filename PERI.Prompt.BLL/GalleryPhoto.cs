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
    public class GalleryPhoto : ISampleData<EF.GalleryPhoto>
    {
        private readonly IUnitOfWork unitOfWork;

        public GalleryPhoto(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.GalleryPhoto args)
        {
            unitOfWork.GalleryPhotoRepository.Add(args);
            await unitOfWork.CommitAsync();
            return args.GalleryId;
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

        public async Task Delete(EF.GalleryPhoto args)
        {
            unitOfWork.GalleryPhotoRepository.Remove(args);
            await unitOfWork.CommitAsync();
        }

        public async Task Edit(EF.GalleryPhoto args)
        {
            var rec = unitOfWork.GalleryPhotoRepository.Entities.First(x => x.GalleryId == args.GalleryId && x.PhotoId == args.PhotoId);
            rec.Title = args.Title;
            rec.Description = args.Description;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.GalleryPhoto>> Find(EF.GalleryPhoto args)
        {
            var res = await (from q in unitOfWork.GalleryPhotoRepository.Entities
                    .Include(x => x.Photo)
                    .Where(x => x.Title.Contains(args.Title ?? x.Title) && x.GalleryId == (args.GalleryId == 0 ? x.GalleryId : args.GalleryId))
                                select q).ToListAsync();

            return res;
        }

        public async Task<EF.GalleryPhoto> Get(EF.GalleryPhoto args)
        {
            var rec = await unitOfWork.GalleryPhotoRepository.Entities
                .Where(x => x.GalleryId == args.GalleryId && x.PhotoId == args.PhotoId)
                .Include(x => x.Gallery)
                .Include(x => x.Photo)
                .FirstOrDefaultAsync();

            return rec;
        }
    }
}
