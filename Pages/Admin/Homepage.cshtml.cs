namespace AttendanceCheckProject.Pages.Admin;

[Authorize(Roles = "Admin")]
public class AdminHomepageModel : PageModel
{
    public Dictionary<string, int> Data { get; set; }
    private readonly UserDbContext _context;

    public AdminHomepageModel(UserDbContext context)
    {
        _context = context;
        Data = new();
    }
    public async Task OnGetAsync()
    {
        var users = _context.Users.AsEnumerable();
        var departments = 
            from u in users
            orderby u.Department
            select u.Department;
        await Task.WhenAll(departments.Select<string, Task>(async (string val) =>
        {
            Data[val] = users.Where(u => u.Department! == val).Count();
            await Task.CompletedTask;
        }));
    }
}