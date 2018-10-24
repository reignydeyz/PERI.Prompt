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
        private IUnitOfWork unitOfWork;

        public Event(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.EventId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.Event args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            unitOfWork.EventRepository.Add(args);
            await unitOfWork.CommitAsync();
            return args.EventId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.EventId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.EventRepository.RemoveRange(unitOfWork.EventRepository.Entities.Where(x => ids.Contains(x.EventId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Event args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Event args)
        {
            var rec = unitOfWork.EventRepository.Entities.First(x => x.EventId == args.EventId);
            rec.Name = args.Name;
            rec.Description = args.Description;
            rec.Location = args.Location;
            rec.Time = args.Time;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.Event>> Find(EF.Event args)
        {
            var res = await (from c in unitOfWork.EventRepository.Entities
                             .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                             where (c.Name.Contains(args.Name ?? string.Empty) || c.Description.Contains(args.Description ?? string.Empty))
                             select c).ToListAsync();

            return res;
        }

        public async Task<EF.Event> Get(EF.Event args)
        {
            var res = await (from c in unitOfWork.EventRepository.Entities
                             .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                             where (c.EventId == args.EventId)
                             select c).FirstOrDefaultAsync();

            return res;
        }

        public async Task<Tuple<EF.Event, bool>> GetModel(int id)
        {
            var rec = await unitOfWork.EventRepository.Entities
                .Include(x => x.EventPhoto).ThenInclude(x => x.Photo)
                .FirstAsync(x => x.EventId == id);

            return new Tuple<EF.Event, bool>(rec, rec.DateInactive == null);
        }
    }
}
