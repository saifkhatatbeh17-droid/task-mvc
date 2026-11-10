using System.ComponentModel.DataAnnotations;

namespace CafeteriaTask8.Models;

public class UserRoleViewModel
{
    public string Id { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Email { get; set; } = "";

    [Required]
    public string Role { get; set; } = "User";
}
