using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        //Temos que declarar uma dependeicia para o context

        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? min, DateTime? max)
        {
            var result = from obj in _context.SalesRecords select obj;
            if(min.HasValue)
            {
                result = result.Where(x => x.Date >= min.Value);
            }

            if(max.HasValue)
            {
                result = result.Where(x => x.Date <= max.Value);
            }

            return await result.Include(x => x.Seller)
                .Include(x => x.Seller.Departament)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Departament, SalesRecord>>> FindByDateGroupingAsync(DateTime? min, DateTime? max)
        {
            var result = from obj in _context.SalesRecords select obj;
            if (min.HasValue)
            {
                result = result.Where(x => x.Date >= min.Value);
            }

            if (max.HasValue)
            {
                result = result.Where(x => x.Date <= max.Value);
            }

            return await result.Include(x => x.Seller)
                .Include(x => x.Seller.Departament)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Departament)
                .ToListAsync();
        }
    }
}
