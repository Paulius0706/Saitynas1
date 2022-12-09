using Microsoft.AspNetCore.Identity;

namespace TimeT.Auth.Model
{
    public class TimeTUser : IdentityUser
    {
        [PersonalData]
        public string? AdditionalInfo { get; set; }
    }
}
