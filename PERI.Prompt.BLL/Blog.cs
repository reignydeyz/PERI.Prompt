using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Blog : ISampleData<EF.Blog>
    {
        private readonly IUnitOfWork unitOfWork;

        public Blog(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.BlogRepository.Entities.Where(x => ids.Contains(x.BlogId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.Blog args)
        {
            args.DateCreated = DateTime.Now;
            args.ModifiedBy = args.CreatedBy;
            args.DateModified = args.DateCreated;
            unitOfWork.BlogRepository.Add(args);
            await unitOfWork.CommitAsync();
            return args.BlogId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.BlogRepository.Entities.Where(x => ids.Contains(x.BlogId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.BlogRepository.RemoveRange(unitOfWork.BlogRepository.Entities.Where(x => ids.Contains(x.BlogId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.Blog args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.Blog args)
        {
            var rec = unitOfWork.BlogRepository.Entities.First(x => x.BlogId == args.BlogId);
            rec.Title = args.Title;
            rec.Body = args.Body;
            rec.ModifiedBy = args.ModifiedBy;
            rec.DateModified = DateTime.Now;
            rec.DateInactive = args.DateInactive;
            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.Blog>> Find(EF.Blog args)
        {
            var res = await (from c in unitOfWork.BlogRepository.Entities
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                             where (c.Title.Contains(args.Title ?? string.Empty) || c.Body.Contains(args.Body ?? string.Empty))
                             && c.CreatedBy == (args.CreatedBy ?? c.CreatedBy)
                             select c).ToListAsync();

            return res;
        }

        /// <summary>
        /// Gets the Blogs within the Category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <param name="blogSortOrderId">Blog Sort Order Id</param>
        /// <returns>List of Blogs</returns>
        public async Task<IEnumerable<EF.Blog>> FindByCategoryId(int id)
        {
            var blogSortOrderId = (await unitOfWork.CategoryRepository.Entities.FirstAsync(x => x.CategoryId == id)).BlogSortOrderId;

            var res = await (from b in unitOfWork.BlogRepository.Entities
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                    join bc in unitOfWork.BlogCategoryRepository.Entities on b.BlogId equals bc.BlogId
                    join c in unitOfWork.CategoryRepository.Entities on bc.CategoryId equals c.CategoryId
                    where c.CategoryId == id
                    && b.VisibilityId == 1
                             select b).ToListAsync();

            switch (blogSortOrderId)
            {
                // Ascending by DateCreated
                case 1:
                    return res.OrderBy(x => x.DateCreated).ToList();
                // Descending by DateCreated
                case 2:
                    return res.OrderByDescending(x => x.DateCreated).ToList();
                // Ascending by DatePublished
                case 3:
                    return res.OrderBy(x => x.DatePublished).ToList();
                // Descending by DatePublished
                default:
                case 4:
                    return res.OrderByDescending(x => x.DatePublished).ToList();
                // Ascending by Title
                case 5:
                    return res.OrderBy(x => x.Title).ToList();
                // Descending by Title
                case 6:
                    return res.OrderByDescending(x => x.Title).ToList();
            }
        }

        public async Task<EF.Blog> Get(EF.Blog args)
        {
            var rec = await unitOfWork.BlogRepository.Entities
                    .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
                    .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
                    .Include(x => x.BlogAttachment).ThenInclude(x => x.Attachment)
                    .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
                    .FirstOrDefaultAsync(x => x.BlogId == args.BlogId);

            return rec;
        }

        public async Task<Tuple<EF.Blog, string, bool, Dictionary<string, bool>>> GetModel(int id)
        {
            var rec = await unitOfWork.BlogRepository.Entities
            .Include(x => x.BlogAttachment).ThenInclude(x => x.Attachment)
            .Include(x => x.BlogPhoto).ThenInclude(x => x.Photo)
            .Include(x => x.BlogTag).ThenInclude(x => x.Tag)
            .Include(x => x.BlogCategory).ThenInclude(x => x.Category)
            .FirstAsync(x => x.BlogId == id);

            var csv = String.Join(",", rec.BlogTag.Select(x => x.Tag.Name));

            var dict = new Dictionary<string, bool>();
            var categories = await (from c in unitOfWork.CategoryRepository.Entities
                             join bc in rec.BlogCategory on c.CategoryId equals bc.CategoryId into jointable
                             from z in jointable.DefaultIfEmpty()
                             select new
                             {
                                 Key = c.Name,
                                 Value = z != null
                             }).ToListAsync();

            return new Tuple<EF.Blog, string, bool, Dictionary<string, bool>>(rec, csv, rec.DateInactive == null, categories.ToDictionary(x => x.Key, x => x.Value));
        }
    }
}
