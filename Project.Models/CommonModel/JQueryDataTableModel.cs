using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models.CommonModel
{
    public class JQueryDataTableModel
    {
        public int length { get; set; }
        public string ordercolumn { get; set; }
        public string sortorder { get; set; }
        public string search { get; set; }
        public int start { get; set; }
    }


    public class UserListFilterModel
    {
        public int length { get; set; }
        public string ordercolumn { get; set; }
        public string sortorder { get; set; }
        public string search { get; set; }
        public int start { get; set; }
        public string role { get; set; }
        public string merchant { get; set; }
        public string branch { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class MerchantListFilterModel
    {
        public int length { get; set; }
        public string ordercolumn { get; set; }
        public string sortorder { get; set; }
        public string search { get; set; }
        public int start { get; set; }

        private string _Date { get; set; }
        public string Date
        {
            get => _Date;
            set
            {
                this._Date = value;
                if (_Date is not null)
                    this.InvoiceDate = DateTime.ParseExact(_Date, "MMMM yyyy", System.Globalization.CultureInfo.InvariantCulture);

            }
        }

        public int ViewType { get; set; }
        public string merchant { get; set; }
        public string branch { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
