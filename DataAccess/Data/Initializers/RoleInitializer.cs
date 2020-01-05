using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data.Initializers
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
           
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("seller") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("seller"));
            }
            if (await roleManager.FindByNameAsync("buyer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("buyer"));
            }
            string adminEmail = "admin@example.com";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                
                string password = "Admin1!";
                IdentityUser admin = new IdentityUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            string sellerEmail = "seller@example.com";
            if (await userManager.FindByNameAsync(sellerEmail) == null)
            {

                string password = "Seller2@";
                IdentityUser seller = new IdentityUser { Email = sellerEmail, UserName = sellerEmail };
                IdentityResult result = await userManager.CreateAsync(seller, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(seller, "seller");
                }
            }
            string buyerEmail = "buyer@example.com";
            if (await userManager.FindByNameAsync(buyerEmail) == null)
            {

                string password = "Buyer3#";
                IdentityUser buyer = new IdentityUser { Email = buyerEmail, UserName = buyerEmail };
                IdentityResult result = await userManager.CreateAsync(buyer, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(buyer, "buyer");
                }
            }
        }
    }
}
