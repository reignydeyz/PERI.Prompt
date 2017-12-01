using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Event : ISampleData<EF.Event>
    {
        EF.SampleDbContext context;

        public Event(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task Activate(int[] ids)
        {
            var res = context.Event.Where(x => ids.Contains(x.EventId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await context.SaveChangesAsync();
        }

        public async Task<int> Add(EF.Event args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            context.Event.Add(args);
            await context.SaveChangesAsync();
            return args.EventId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = context.Event.Where(x => ids.Contains(x.EventId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            context.Event.RemoveRange(context.Event.Where(x => ids.Contains(x.EventId)));
            await context.SaveChangesAsync();
        }

        public Task Delete(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Event args)
        {
            var rec = context.Event.First(x => x.EventId == args.EventId);
            rec.Name = args.Name;
            rec.Description = args.Description;
            rec.Location = args.Location;
            rec.Time = args.Time;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EF.Event>> Find(EF.Event args)
        {
            var res = await (from c in context.Event
                             .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                             where (c.Name.Contains(args.Name ?? string.Empty) || c.Description.Contains(args.Description ?? string.Empty))
                             select c).ToListAsync();

            return res;
        }

        public async Task<EF.Event> Get(EF.Event args)
        {
            var res = await (from c in context.Event
                             .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                             where (c.EventId == args.EventId)
                             select c).FirstOrDefaultAsync();

            return res;
        }

        public async Task<Tuple<EF.Event, bool>> GetModel(int id)
        {
            var rec = await context.Event
                .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                .FirstAsync(x => x.EventId == id);

            return new Tuple<EF.Event, bool>(rec, rec.DateInactive == null);
        }
    }
}
