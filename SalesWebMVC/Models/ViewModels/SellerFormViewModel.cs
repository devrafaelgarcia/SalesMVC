using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Models.ViewModels
{
    public class SellerFormViewModel
    {
        public Seller Seller { get; set; } //Precisamos ter um vendedor nesses dados e uma list de departamento
        public ICollection<Departament> Departaments { get; set; } = new List<Departament>();
    }
}
