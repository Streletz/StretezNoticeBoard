using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSL.Site
{
    public class SiteUserManager : BaseManager
    {
        public SiteUserManager(ApplicationDbContext context) : base(context)
        {
            
        }

        public IdentityUser FindById(String id)
        {
            return _context.Users.First(x => x.Id == id);
        }

        public IEnumerable<IdentityUser> findAll()
        {
            return _context.Users.OrderBy(x => x.UserName);
        }
    }
}
