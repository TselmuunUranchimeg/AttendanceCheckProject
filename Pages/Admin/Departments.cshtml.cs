namespace AttendanceCheckProject.Pages.Admin;

[Authorize(Roles = "Admin")]
public class AdminTeamsModel: PageModel
{
    private readonly UserManager<UserModel> _userManager;
    public Dictionary<string, IEnumerable<string>> Data { get; set; } = new();

    public AdminTeamsModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }
    public async Task OnGetAsync()
    {
        var users = _userManager.Users.AsEnumerable();
        var departments = 
            from u in users
            orderby u.Department
            where u.Department != String.Empty
            select u.Department;
        foreach(string department in departments)
        {
            var values = await GetEmployeeNamesFromDepartment(users, department);
            Data[department] = values;
        }
    }

    private async Task<IEnumerable<string>> GetEmployeeNamesFromDepartment(
        IEnumerable<UserModel> users, string name)
    {
        return await Task.Run<IEnumerable<string>>(() =>
        {
            return from u in users where u.Department == name select u.UserName;
        });
    }
}