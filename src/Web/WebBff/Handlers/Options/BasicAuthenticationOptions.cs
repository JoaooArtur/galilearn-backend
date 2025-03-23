using System.ComponentModel.DataAnnotations;

namespace WebBff.Handlers.Options
{
    public abstract record BasicAuthenticationOptions
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
