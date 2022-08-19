namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Employee")]
public class CheckDayModel: PageModel
{
    private readonly AttendanceDbContext _context;

    [BindProperty(SupportsGet = true)]
    public int DayId { get; set; }

    public Attendance Target { get; set; } = new();

    [BindProperty]
    [Required(ErrorMessage = "Please input your reason!")]
    public string Reason { get; set; } = "";

    public CheckDayModel(AttendanceDbContext context)
    {
        _context = context;
    }
    public IActionResult OnGet()
    {
        var item = 
            from a in _context.Attendance
            where a.ID == DayId
            select a;
        if (item.FirstOrDefault() is null)
        {
            return RedirectToPage("/AccessDenied");
        }
        Target = item.FirstOrDefault()!;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            Target.Reason = Reason;
            _context.Attendance!.Update(Target);
            return RedirectToPage("/Index");
        }
        return Page();
    }
}