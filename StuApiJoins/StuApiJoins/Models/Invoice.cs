using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StuApiJoins.Models
{
    public class Invoice
    {
        public int INV_NUMBER { get; set; }
        public DateTime INV_DATE { get; set; }
        public List<LineItem> InvoiceLineItems { get; set; }
    }
}
