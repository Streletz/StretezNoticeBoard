using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Admin
{
    public class UserAdminManager : BaseManager
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public UserAdminManager(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager) : base(context)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public IEnumerable<IdentityUser> FindAll()
        {
            return _context.Users.OrderBy(x => x.UserName);
        }

        public IdentityUser FindById(String id)
        {
            return _context.Users.First(x => x.Id == id);
        }

        public IEnumerable<IdentityRole> FindAllRoles()
        {
            return _context.Roles.OrderBy(x => x.Name);
        }

        public async Task Add(String password, IdentityUser user, String roleId)
        {
            IdentityResult result = await _userManager.CreateAsync(user, password).ConfigureAwait(false);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, roleId).ConfigureAwait(false);
            }
        }

        public async Task Update(String roleId, IdentityUser rawUser)
        {
            IdentityResult result = await _userManager.UpdateAsync(rawUser).ConfigureAwait(false);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(rawUser, roleId).ConfigureAwait(false);
            }
        }

        public void Delete(string id)
        {
            IdentityUser rawUser = FindById(id);
            _userManager.DeleteAsync(rawUser);
        }
    }
}
