using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Identity;
//using WebApp.Data.Models;

namespace WebApp.Pages.AppIdentityUsers
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly WebApp.Data.ApplicationDbContext _context;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public IndexModel(WebApp.Data.ApplicationDbContext context, SignInManager<AppIdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _context = context;
        }


        public IList<AppIdentityUser> AppIdentityUser { get; set; } = default!;

        [BindProperty]
        public bool CheckedAll { get; set; }


        [BindProperty]
        public List<bool> IsChecked { get; set; }



        public async Task OnGetAsync()
        {
            if (_context.AppIdentityUser != null)
            {
                AppIdentityUser = await _context.AppIdentityUser.ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var username = HttpContext.User.Identity.Name;
            var users = await _context.AppIdentityUser.ToListAsync();
            for (int i = 0; i < users.Count(); i++)
            {
                if (IsChecked[i])
                {
                    _context.AppIdentityUser.Remove(users[i]);
                    await _context.SaveChangesAsync();
                    if (users[i].UserName == username)
                    {
                        await Logout();
                    }
                }
            }
            return RedirectToPage("./Index");
        }

        
        public async Task<IActionResult> OnPostBlockAsync()
        {

            var username = HttpContext.User.Identity.Name;

            var users = await _context.AppIdentityUser.ToListAsync();
            for (int i = 0; i < users.Count(); i++)
            {
                if (IsChecked[i])
                {
                    users[i].LockoutEnabled = true;
                    users[i].LockoutEnd = DateTime.UtcNow.AddYears(100);
                    _context.Attach(users[i]).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                        if (users[i].UserName == username)
                        {
                            await Logout();
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(users[i].Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
            }
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostUnblockAsync()
        {
            var users = await _context.AppIdentityUser.ToListAsync();
            for (int i = 0; i < users.Count(); i++)
            {
                if (IsChecked[i])
                {
                    users[i].LockoutEnabled = false;
                    users[i].LockoutEnd = DateTime.Now;
                    _context.Attach(users[i]).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(users[i].Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                }
            }
            return RedirectToPage("./Index");
        }


        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
            return RedirectToPage("./Identity/Account/Login");
        }


        private bool UserExists(string id)
        {
            return (_context.AppIdentityUser?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
