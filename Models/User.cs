namespace Models;

public class UserModel: IdentityUser
{
    [Required]
    public int Age { get; set; }

    [Required]
    [EmailAddress]
    public override string? Email { get; set; } = "";

    [Required]
    public string? Department { get; set; } = "";
}