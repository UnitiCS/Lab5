using Bakery.Models;
using Bakery.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id);

            List<UserViewModel> userViewModel = new List<UserViewModel>();

            string urole = "";
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count() > 0)
                {
                    urole = userRoles[0] ?? "";
                }

                userViewModel.Add(
                    new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        RoleName = urole

                    });

            }

            return View(userViewModel);
        }

        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel user = new CreateUserViewModel();

            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                var role = model.UserRole;
                if (role.Count() > 0)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
           
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            string userRole = "";
            if (userRoles.Count() > 0)
            {
                userRole = userRoles[0] ?? "";
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                UserRole = userRole
            };
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name", model.UserRole);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // Проверяем, что текущий пользователь не пытается изменить свою собственную роль и профиль
                    if (User.IsInRole("SuperAdmin") && user.Id == _userManager.GetUserId(User))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не можете изменить свою роль или редактировать свой профиль..");
                        return View(model);
                    }

                    var oldRoles = await _userManager.GetRolesAsync(user);

                    // Проверка наличия ролей
                    if (oldRoles.Count() > 0)
                    {
                        // Удаление всех старых ролей
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                        // Если новая роль - SuperAdmin, то текущему суперадмину присваивается роль Admin
                        if (model.UserRole == "SuperAdmin")
                        {
                            var currentSuperAdmins = await _userManager.GetUsersInRoleAsync("SuperAdmin");

                            // Check if the current user is a super admin and not the one being edited
                            if (User.IsInRole("SuperAdmin") && currentSuperAdmins.Any(u => u.Id != user.Id))
                            {
                                // Log out the current user (revoke the session)
                                await _signInManager.SignOutAsync();
                            }

                            foreach (var currentSuperAdmin in currentSuperAdmins)
                            {
                                await _userManager.RemoveFromRoleAsync(currentSuperAdmin, "SuperAdmin");
                                await _userManager.AddToRoleAsync(currentSuperAdmin, "Admin");

                                // Обновление информации о текущем суперадмине в базе данных
                                await _userManager.UpdateAsync(currentSuperAdmin);
                            }
                        }
                    }

                    // Добавление новой роли
                    await _userManager.AddToRoleAsync(user, model.UserRole);

                    user.Email = model.Email;
                    user.UserName = model.UserName;

                    // Обновление информации о пользователе в базе данных
                    await _userManager.UpdateAsync(user);

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }





        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}