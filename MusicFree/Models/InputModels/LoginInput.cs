using System.ComponentModel.DataAnnotations;

namespace MusicFree.Models.AutenthicationModels
{
    public class LoginInput
    {

        [Required(ErrorMessage = "User Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
