namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Admin,Employee")]
public class EmployeeDetailsModel: PageModel
{
    private readonly UserManager<UserModel> _userManager;
    public UserModel UserData { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? EmployeeName { get; set; } = "";
    public EmployeeDetailsModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IActionResult> OnGet()
    {
        UserModel user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Employee"))
        {
            if (user.UserName != EmployeeName)
            {
                return RedirectToPage("/AccessDenied");
            }
        }
        UserData = await _userManager.FindByNameAsync(EmployeeName);
        return Page();
    }
}