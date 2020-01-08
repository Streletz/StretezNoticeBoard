using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StreletzNoticeBoard.Areas.Admin.Components;

namespace StreletzNoticeBoard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;


        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: User
        public ActionResult Index()
        {
            IEnumerable<IdentityUser> rawUsers = _context.Users.OrderBy(x => x.UserName);
            //IEnumerable<UserViewModel> users = rawUsers.Se;
            IEnumerable<UserViewModel> users = new LinkedList<UserViewModel>();
            foreach (IdentityUser rawuser in rawUsers)
            {
                UserViewModel user = UserViewModel.Parse(rawuser, _context, _userManager);
                ((LinkedList<UserViewModel>)users).AddLast(user);
            }
            int a = rawUsers.Count();
            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(string id)
        {
            IdentityUser rawUser = _context.Users.First(x => x.Id == id);
            UserViewModel model = new UserViewModel
            {
                Id = rawUser.Id,
                UserName = rawUser.UserName,
                Role = _userManager.GetRolesAsync(rawUser).Result.First(),
                Roles = _context.Roles.OrderBy(x => x.Name)
            };
            return View(model);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            UserViewModel model = new UserViewModel
            {
                Roles = _context.Roles.OrderBy(x => x.Name)
            };
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserViewModel viewModel)
        {
            try
            {
                // TODO: Add insert logic here
                IdentityUser user = new IdentityUser
                {
                    Email = viewModel.UserName,
                    UserName = viewModel.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(user, viewModel.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, viewModel.Role).ConfigureAwait(false);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(string id)
        {
            IdentityUser rawUser = _context.Users.First(x => x.Id == id);
            UserViewModel model = new UserViewModel
            {
                Id = rawUser.Id,
                UserName = rawUser.UserName,
                Role = _userManager.GetRolesAsync(rawUser).Result.First(),
                Roles = _context.Roles.OrderBy(x => x.Name)
            };
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, UserViewModel viewModel)
        {
            try
            {
                // TODO: Add update logic here
                IdentityUser rawUser = _context.Users.First(x => x.Id == id);
                rawUser.UserName = viewModel.UserName;
                rawUser.Email = viewModel.UserName;
                IdentityResult result = await _userManager.UpdateAsync(rawUser).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(rawUser, viewModel.Role).ConfigureAwait(false);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            IdentityUser rawUser = _context.Users.First(x => x.Id == id);
            UserViewModel model = new UserViewModel
            {
                Id = rawUser.Id,
                UserName = rawUser.UserName,
                Role = _userManager.GetRolesAsync(rawUser).Result.First(),
                Roles = _context.Roles.OrderBy(x => x.Name)
            };
            return View(model);
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id, UserViewModel viewModel)
        {
            try
            {
                IdentityUser rawUser = _context.Users.First(x => x.Id == id);
                _userManager.DeleteAsync(rawUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}