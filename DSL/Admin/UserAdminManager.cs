using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DSL.Admin
{
    public class UserAdminManager : BaseManager
    {
        public UserAdminManager(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<IdentityUser> FindAll()
        {
            return _context.Users.OrderBy(x => x.UserName);
        }

        public IdentityUser FindById(String id)
        {
            return _context.Users.First(x => x.Id == id);
        }
    }
}
