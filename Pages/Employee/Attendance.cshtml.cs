namespace AttendanceCheckProject.Pages.Employee;

public class EmployeeAttendanceModel: PageModel
{
    private readonly AttendanceDbContext _context;
    private readonly UserManager<UserModel> _userManager;
    public IEnumerable<Attendance> Attendances { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Month { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }

    public Dictionary<int, string> MonthNames { get; } = new();

    public EmployeeAttendanceModel(AttendanceDbContext context, UserManager<UserModel> userManager)
    {
        _context = context;
        Attendances = default!;
        _userManager = userManager;
        MonthNames.Add(1, "January");
        MonthNames.Add(2, "February");
        MonthNames.Add(3, "March");
        MonthNames.Add(4, "April");
        MonthNames.Add(5, "May");
        MonthNames.Add(6, "June");
        MonthNames.Add(7, "July");
        MonthNames.Add(8, "August");
        MonthNames.Add(9, "September");
        MonthNames.Add(10, "October");
        MonthNames.Add(11, "November");
        MonthNames.Add(12, "December");
    }
    public async Task OnGet()
    {
        UserModel user = await _userManager.GetUserAsync(User);
        Attendances = 
            from item in _context.Attendance?.AsEnumerable()
            where item.EmployeeId == user.Id && item.Date.Month == Month && item.Date.Year == Year
            select item;
    }
}