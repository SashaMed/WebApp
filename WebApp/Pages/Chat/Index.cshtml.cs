using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Identity;
using WebApp.Data;

namespace WebApp.Pages.Chat
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ILogger<IndexModel> _logger;
        private readonly WebApp.Data.ApplicationDbContext _context;

        [BindProperty]
        public string MyUser { get; set; }

        [BindProperty]
        public List<SelectListItem> Users { get; set; }

        [BindProperty]
        public Message Message { get; set; } = default!;

        public IndexModel(ILogger<IndexModel> logger, UserManager<AppIdentityUser> userManager,
            WebApp.Data.ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }



        public void OnGet()
        {
            Users = _userManager.Users.ToList()
                .Select(a => new SelectListItem { Text = a.UserName, Value = a.UserName })
                .OrderBy(s => s.Text).ToList();

            MyUser = User.Identity.Name;
        }

        

        public async Task<IActionResult> OnPostBaseAsync()
        {
            if (!ModelState.IsValid || _context.Messages == null || Message == null)
            {
                return Page();
            }

            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}