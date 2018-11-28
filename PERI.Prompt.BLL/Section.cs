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
    public class Section : ISampleData<EF.Section>
    {
        private readonly IUnitOfWork unitOfWork;

        public Section(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Section args)
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

        public Task Delete(EF.Section args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Section args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Section>> Find(EF.Section args)
        {
            var res = await unitOfWork.SectionRepository.Entities
                .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.SectionProperty).ThenInclude(x => x.SectionItemProperty)
                .Where(x => x.TemplateId == args.TemplateId
                && x.Name.Contains(args.Name ?? string.Empty))
                .ToListAsync();

            return res;
        }

        public async Task<EF.Section> Get(EF.Section args)
        {
            var rec = await unitOfWork.SectionRepository.Entities
                .Include(x => x.SectionItem).ThenInclude(x => x.SectionItemPhoto).ThenInclude(x => x.Photo)
                .Include(x => x.SectionProperty).ThenInclude(x => x.SectionItemProperty)
                .FirstOrDefaultAsync(x => x.SectionId == args.SectionId);

            return rec;
        }
    }
}
