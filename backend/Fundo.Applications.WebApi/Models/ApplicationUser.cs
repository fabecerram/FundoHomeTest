using Microsoft.AspNetCore.Identity;

namespace Fundo.Applications.WebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
