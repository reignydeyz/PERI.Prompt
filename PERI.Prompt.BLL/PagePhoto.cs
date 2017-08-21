using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PERI.Prompt.BLL
{
    public class PagePhoto : ISampleData<EF.PagePhoto>
    {
        EF.SampleDbContext context;

        public PagePhoto(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(EF.PagePhoto args)
        {
            context.PagePhoto.Add(args);
            await context.SaveChangesAsync();
            return args.PageId;
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

        public Task Delete(EF.PagePhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.PagePhoto args)
        {
            var bp = context.PagePhoto.FirstOrDefault(x => x.PageId == args.PageId && x.PhotoId == args.PhotoId);

            if (bp == null)
            {
                await Add(args);
            }
        }

        public async Task<IEnumerable<EF.PagePhoto>> Find(EF.PagePhoto args)
        {
            var res = await context.PagePhoto.Where(x => x.PageId == args.PageId).ToListAsync();

            return res;
        }

        public Task<EF.PagePhoto> Get(EF.PagePhoto args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.PagePhoto>> Get(int[] ids)
        {
            var res = await context.PagePhoto.Where(x => ids.Contains(x.PageId)).ToListAsync();

            return res;
        }
    }
}
