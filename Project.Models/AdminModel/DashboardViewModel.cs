using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.AdminModel
{
    public class DashboardViewModel
    {

        public int TotalUsers { get; set; }
        public int MonthlyNewUsers { get; set; }

        public string UserProfile{ get; set; }
        public string UserName{ get; set; }
        public List<MonthlyUsers> LstMonthlyUsers { get; set; }
    }

    public class MonthlyUsers
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal UserCount { get; set; }
     
    }
}
