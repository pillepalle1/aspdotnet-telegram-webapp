using Microsoft.AspNetCore.Identity;

namespace Pillepalle1.TelegramWebapp.Model.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public long TelegramNativeId { get; set; }
        public string TelegramUserName { get; set; }
        public string FirstName { get; set; }
        public string PhotoUrl { get; set; }
    }
}