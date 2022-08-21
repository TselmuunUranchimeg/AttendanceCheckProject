namespace AttendanceCheckProject.Pages.Employee;

public class EditReasonModel: PageModel
{
    public class AttendanceDTO
    {
        public string Status { get; set; } = "";
        public DateTime Date { get; set; }
    }
    private readonly ICheckRole _checkRole;
    private readonly AttendanceDbContext _context;
    public EditReasonModel(ICheckRole checkRole, AttendanceDbContext context)
    {
        _checkRole = checkRole;
        _context = context;
        Target = new();
    }

    [BindProperty(SupportsGet = true)]
    public string EmployeeId { get; set; } = "";

    [BindProperty(SupportsGet = true)]
    public int AttId { get; set; } = 0;

    [BindProperty]
    [Required]
    public string Reason { get; set; } = "";

    public AttendanceDTO Target { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (await _checkRole.EmployeeIsAllowed(User, EmployeeId)) {
            var att = from a in _context.Attendance where a.ID == AttId select a;
            var data = att.FirstOrDefault();
            if (data is not null)
            {
                Target.Date = data.Date;
                Target.Status = data.Status;
                Reason = data.Reason ?? String.Empty;
                return Page();
            }
        }
        return RedirectToPage("/AccessDenied");
    }

    public async Task<IActionResult> OnPost()
    {
        if (ModelState.IsValid)
        {
            var att = from a in _context.Attendance where a.ID == AttId select a;
            att.FirstOrDefault()!.Reason = Reason;
            await _context.SaveChangesAsync();
            return RedirectToPage("/Index");
        }
        return Page();
    }
}