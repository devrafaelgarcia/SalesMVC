using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Data
{
    public class SeedingService
    {
        private SalesWebMVCContext _context;

        public SeedingService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Departament.Any() ||
                _context.Seller.Any() ||
                _context.SalesRecords.Any()) return; //DB has been seeded

            Departament d1 = new Departament(1, "Computers");
            Departament d2 = new Departament(2, "Electronics");
            Departament d3 = new Departament(3, "Tools");

            Seller s1 = new Seller(1, "Rafael", "rafael@hotmail.com", 1000.0, new DateTime(1999, 9, 9), d1);
            Seller s2 = new Seller(2, "Laura", "Laura@hotmail.com", 2000.0, new DateTime(2000, 3, 1), d2);
            Seller s3 = new Seller(3, "Fabio", "fabiao@hotmail.com", 3000.0, new DateTime(1999, 9, 9), d3);

            SalesRecord r1 = new SalesRecord(1, new DateTime(2018, 9, 9), 300, SaleStatus.Pading, s1);
            SalesRecord r2 = new SalesRecord(2, new DateTime(2018, 7, 9), 1000, SaleStatus.Canceled, s2);
            SalesRecord r3 = new SalesRecord(3, new DateTime(2018, 7, 10), 2000, SaleStatus.Billed, s3);

            //Include Obj from DB

            _context.Departament.AddRange(d1, d2, d3);

            _context.Seller.AddRange(s1, s2, s3);

            _context.SalesRecords.AddRange(r1, r2, r3);

            _context.SaveChanges();
        }
    }
}
