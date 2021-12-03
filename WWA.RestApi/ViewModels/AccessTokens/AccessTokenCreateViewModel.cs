using System.ComponentModel.DataAnnotations;

namespace WWA.RestApi.ViewModels.AccessTokens
{
    public class AccessTokenCreateViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
