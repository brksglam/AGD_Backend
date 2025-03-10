using Microsoft.AspNetCore.Identity;

namespace Fresh.Api.Models;

public class AppUser : IdentityUser

{
    
    public string Ad { get; set; }
    public string Soyad { get; set; }


}
