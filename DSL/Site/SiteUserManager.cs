using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSL.Site
{
    public class SiteUserManager : DslObject
    {
        public SiteUserManager(ApplicationDbContext context) : base(context)
        {
            _context.Users.First(x => x.Id == user.Id)
        }

        public IdentityUser FindById()
        {
            return _context.Users.First(x => x.Id == user.Id);
        }
    }
}
