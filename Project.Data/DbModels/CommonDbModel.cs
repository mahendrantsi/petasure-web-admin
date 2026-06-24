using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Core.Extension;
using Project.Data.DBEntities;
using Project.Data.ExtendedDBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.DbModels
{
    public partial class DbModel
    {
        public void CommonOnModelCreating(ModelBuilder builder)
        {

            
        }

    }
}
