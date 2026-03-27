using System.ComponentModel.DataAnnotations;

namespace MF_API.Models
{
    public class AuthenticateDto
    {

        [Required]
        public int Id { get; set; }
        [Required]
        public string Password { get; set; }


    }
}
