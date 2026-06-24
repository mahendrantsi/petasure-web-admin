using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DBEntities
{
    [Keyless]
    public class DashboardDetailsResult
    {
        public int UserCount { get; set; }
        public int TransactionCount { get; set; }
        public int CurrencyCount { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
