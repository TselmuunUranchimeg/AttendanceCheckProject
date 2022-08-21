namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Employee,Admin")]
public class CheckDayModel: PageModel
{
    private readonly AttendanceDbContext _context;
    private readonly ICheckRole _checkRole;

    [BindProperty(SupportsGet = true)]
    public string EmployeeId { get; set; } = "";

    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Month { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Day { get; set; }

    public Attendance[]? Target { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please input your reason!")]
    public string Reason { get; set; } = "";

    public CheckDayModel(AttendanceDbContext context, ICheckRole checkRole)
    {
        _context = context;
        _checkRole = checkRole;
    }
    public async Task<IActionResult> OnGet()
    {
        if (await _checkRole.EmployeeIsAllowed(User, EmployeeId)) {
            var data = 
                from a in _context.Attendance
                where a.EmployeeId == EmployeeId
                where a.Date.Year == Year && a.Date.Month == Month && a.Date.Day == Day
                select a;
            Target = data.ToArray();
            return Page();
        }
        return RedirectToPage("/AccessDenied");
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }
}