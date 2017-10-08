using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Gallery : ISampleData<EF.Gallery>
    {
        EF.SampleDbContext context;

        public Gallery(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Gallery args)
        {
            throw new NotImplementedException();
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

        public Task Delete(EF.Gallery args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Gallery args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Gallery>> Find(EF.Gallery args)
        {
            var res = await (from c in context.Gallery
                        .Include(x => x.GalleryPhoto).ThenInclude(x => x.Photo)
                        where c.Name.Contains(args.Name ?? string.Empty)
                        && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                        select c).ToListAsync();

            return res;
        }

        public async Task<EF.Gallery> Get(EF.Gallery args)
        {
            var rec = await context.Gallery
                .Include(x => x.GalleryPhoto).ThenInclude(x => x.Photo)
                .FirstOrDefaultAsync(x => x.GalleryId == args.GalleryId);

            return rec;
        }
    }
}
