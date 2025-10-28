using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class LoginViewModel
    {
        // [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter User Name")]
        public string? UserName { get; set; } = "admin";

        //  [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter Password")]
        public string? Password { get; set; } = "password";
    }
}
