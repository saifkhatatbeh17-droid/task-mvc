using System.ComponentModel.DataAnnotations;

namespace CafeteriaTask8.Models;

public class RegisterViewModel
{
    [Required]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; } = "";
}
