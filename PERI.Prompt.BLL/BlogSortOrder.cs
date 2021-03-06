﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class BlogSortOrder : ISampleData<EF.BlogSortOrder>
    {
        private readonly IUnitOfWork unitOfWork;

        public BlogSortOrder(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.BlogSortOrder args)
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

        public Task Delete(EF.BlogSortOrder args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.BlogSortOrder args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.BlogSortOrder>> Find(EF.BlogSortOrder args)
        {
            var res = await(from c in unitOfWork.BlogSortOrderRepository.Entities
                            where c.Name.Contains(args.Name ?? string.Empty)
                            select c).ToListAsync();

            return res;
        }

        public Task<EF.BlogSortOrder> Get(EF.BlogSortOrder args)
        {
            throw new NotImplementedException();
        }
    }
}
