using System.ComponentModel.DataAnnotations;

namespace TimeT.Auth.Model.AuthDtos
{
    public record RegisterUserDto([Required] string UserName, [EmailAddress][Required] string Email, [Required] string Password);
    //public class RegisterUserDto
    //{
    //    [Required]
    //    public string UserName;
        
    //    [EmailAddress]
    //    [Required]
    //    public string Email; 
        
    //    [Required]
    //    public string Password;
    //}
}
