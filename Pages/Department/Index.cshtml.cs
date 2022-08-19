namespace AttendanceCheckProject.Pages.Department;

[Authorize(Roles = "Admin,Employee")]
public class DepartmentModel: PageModel
{
    [BindProperty(SupportsGet = true)]
    public string DepartmentName { get; set; } = "";
    public IEnumerable<string> Names { get; set; }
    private readonly UserManager<UserModel> _userManager;

    public DepartmentModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
        Names = default!;
    }

    public async Task<IActionResult> OnGet()
    {
        var employees = 
            from u in _userManager.Users
            where u.Department == DepartmentName
            select u.UserName;
        UserModel user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Employee"))
        {
            if (!employees.Any(i => i == user.UserName))
            {
                return RedirectToPage("/AccessDenied");
            }
        }
        Names = employees.AsEnumerable();
        return Page();
    }
}