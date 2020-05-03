using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace DSL.Admin
{
    public class NoticesAdminManager : DslObject
    {
        public NoticesAdminManager(ApplicationDbContext context) : base(context)
        {
        }
    }
}
