using Microsoft.AspNetCore.Identity;

namespace WebApp.Data.Identity
{
    public class AppIdentityUser : IdentityUser
    {

        public long AppId { get; set; }

        public DateTime RegistrDate { get; set; }

        public DateTime LasLoginDate { get; set; }

        public bool IsChecked { get; set; }
    }
}
