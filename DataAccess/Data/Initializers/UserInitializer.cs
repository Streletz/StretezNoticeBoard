using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Initializers
{
    public class UserInitializer
    {
        private const string AdminRoleName = "admin";
        private const string SellerRoleName = "seller";
        private const string BuyerRoleName = "buyer";
        private const string AdminUserEmail = "admin@example.com";
        private const string SellerUserEmail = "seller@example.com";
        private const string BuyerUserEmail = "buyer@example.com";
        private const string AdminUserPassword = "Admin1!";
        private const string SellerUserPassword = "Seller2@";
        private const string BuyerUserPassword = "Buyer3#";

        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await CreateRole(roleManager, AdminRoleName);
            await CreateRole(roleManager, SellerRoleName);
            await CreateRole(roleManager, BuyerRoleName);
            await CreateUser(userManager, AdminUserEmail, AdminUserPassword, AdminRoleName);
            await CreateUser(userManager, SellerUserEmail, SellerUserPassword, SellerRoleName);
            await CreateUser(userManager, BuyerUserEmail, BuyerUserPassword, BuyerRoleName);
        }

        private static async Task CreateUser(UserManager<IdentityUser> userManager, string userEmail, string userPassword, string roleName)
        {
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                IdentityUser admin = new IdentityUser { Email = userEmail, UserName = userEmail };
                IdentityResult result = await userManager.CreateAsync(admin, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, roleName);
                }
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
