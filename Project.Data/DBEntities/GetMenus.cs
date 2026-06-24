using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.DBEntities
{
    [Keyless]
    public class GetMenus
    {
        public long Id { get; set; }

        public string ParentMenu { get; set; }

        public string MenuName { get; set; }

        public string DisplayName { get; set; }

        public string Url { get; set; }

        public bool? IsActive { get; set; }
    }
}
