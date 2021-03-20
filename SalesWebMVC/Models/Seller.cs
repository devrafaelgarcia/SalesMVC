using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public double BaseSalary { get; set; }
        public DateTime BirthDate { get; set; }
        public Departament Departament { get; set; }
        public int DepartamentId { get; set; } //Obriga que ele deve existir
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, double baseSalary, DateTime birthDate, Departament departament)
        {
            Id = id;
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            BirthDate = birthDate;
            Departament = departament;
        }


        public void RemoveSales(SalesRecord salesRecord)
        {
            Sales.Remove(salesRecord);
        }

        public void AddSales(SalesRecord salesRecord)
        {
            Sales.Add(salesRecord);
        }

        public double TotalSales(DateTime intinial, DateTime final)
        {
            //Linq 

            return Sales.Where(s => s.Date >= intinial && s.Date <= final)
                .Sum(s => s.Amount);
        }

    }   
}
