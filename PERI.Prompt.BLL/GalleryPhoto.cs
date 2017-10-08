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
        EF.SampleDbContext context;

        public GalleryPhoto(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.GalleryPhoto args)
        {
            context.GalleryPhoto.Add(args);
            await context.SaveChangesAsync();
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
            context.GalleryPhoto.Remove(args);
            await context.SaveChangesAsync();
        }

        public async Task Edit(EF.GalleryPhoto args)
        {
            var rec = context.GalleryPhoto.First(x => x.GalleryId == args.GalleryId && x.PhotoId == args.PhotoId);
            rec.Title = args.Title;
            rec.Description = args.Description;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.GalleryPhoto>> Find(EF.GalleryPhoto args)
        {
            var res = await (from q in context.GalleryPhoto
                    .Include(x => x.Photo)
                    .Where(x => x.Title.Contains(args.Title ?? x.Title) && x.GalleryId == (args.GalleryId == 0 ? x.GalleryId : args.GalleryId))
                                select q).ToListAsync();

            return res;
        }

        public async Task<EF.GalleryPhoto> Get(EF.GalleryPhoto args)
        {
            var rec = await context.GalleryPhoto
                .Where(x => x.GalleryId == args.GalleryId && x.PhotoId == args.PhotoId)
                .Include(x => x.Gallery)
                .Include(x => x.Photo)
                .FirstOrDefaultAsync();

            return rec;
        }
    }
}
