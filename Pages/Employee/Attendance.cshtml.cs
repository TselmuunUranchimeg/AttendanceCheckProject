namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Admin,Employee")]
public class EmployeeAttendanceModel: PageModel
{
    private readonly AttendanceDbContext _context;
    private readonly ICheckRole _checkRole;

    [BindProperty(SupportsGet = true)]
    public int Month { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }

    [BindProperty(SupportsGet = true)]
    public string EmployeeId { get; set; } = "";

    public EmployeeAttendanceModel(
        AttendanceDbContext context, UserManager<UserModel> userManager, ICheckRole checkRole)
    {
        _context = context;
        _checkRole = checkRole;
    }
    public async Task<IActionResult> OnGet()
    {
        if (await _checkRole.EmployeeIsAllowed(User, EmployeeId))
        {
            return Page();
        }
        return RedirectToPage("/AccessDenied");
    }
}