using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StuApiJoins.Models
{
    public class Customer
    {
        public int Cus_Code { get; set; }
        public string Cus_Fname { get; set; }
        public string Cus_Lname { get; set; }
        public List<Invoice> Cus_Invoices { get; set; }
    }
}
