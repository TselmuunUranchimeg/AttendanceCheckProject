using Services.AttendanceCheck;

namespace AttendanceCheckProject.Pages;

[Authorize(Roles = "Employee,Admin")]
public class HomepageModel: PageModel
{
    private readonly AttendanceDbContext _context;
    private readonly UserManager<UserModel> _userManager;
    private readonly ILogger<HomepageModel> _logger;
    private readonly IAttendanceCheck _attendanceCheck;
    public IEnumerable<Attendance> RecentActivities { get; set; }
    
    public HomepageModel(
        AttendanceDbContext context, 
        UserManager<UserModel> userManager, 
        ILogger<HomepageModel> logger,
        IAttendanceCheck attendanceCheck)
    {
        _userManager = userManager;
        _context = context;
        RecentActivities = new List<Attendance> {};
        _logger = logger;
        _attendanceCheck = attendanceCheck;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        UserModel user = await _userManager.GetUserAsync(User);
        var list = 
            from item in _context.Attendance?.AsEnumerable()
            where item.EmployeeId == user.Id
            select item;
        RecentActivities = list.Take(10);
        return Page();
    }

    public async Task<IActionResult> OnPostCheckIn()
    {
        try
        {
            UserModel user = await _userManager.GetUserAsync(User);
            var newAttendance = _attendanceCheck.Check(user, DateTime.Now, StatusEnum.CheckIn);
            _context.Attendance?.Add(newAttendance);
            await _context.SaveChangesAsync();
            return await OnGetAsync();
        }
        catch (CustomException e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return await OnGetAsync();
        }
    }
    public async Task<IActionResult> OnPostCheckOut()
    {
        try
        {
            UserModel user = await _userManager.GetUserAsync(User);
            var newAttendance = _attendanceCheck.Check(user, DateTime.Now, StatusEnum.CheckOut);
            _context.Attendance?.Add(newAttendance);
            await _context.SaveChangesAsync();
            return await OnGetAsync();
        }
        catch (CustomException e)
        {
            ViewData["ErrorMessage"] = e.Message;
            return await OnGetAsync();
        }
    }
}