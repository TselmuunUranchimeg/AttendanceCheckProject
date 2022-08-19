namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Admin, Employee")]
public class EmployeeEditModel: PageModel
{
    public class InputModel
    {
        [Required]
        public string UserName { get; set; } = "";

        [Required]
        public int Age { get; set; }

        [Required]
        public string Department { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
    private readonly UserManager<UserModel> _userManager;
    public Attendance Target { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string EmployeeName { get; set; } = "";

    [BindProperty]
    public InputModel Input { get; set; }

    public EmployeeEditModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
        Input = new();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        UserModel currentUser = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(currentUser);
        if (roles.Contains("Employee"))
        {
            if (currentUser.UserName != EmployeeName)
            {
                return RedirectToPage("/AccessDenied");
            }
        }
        var user = _userManager.Users.Where(l => l.UserName == EmployeeName).FirstOrDefault();
        if (user is null)
        {
            return RedirectToPage("/Index");
        }
        Input.Age = user.Age;
        Input.UserName = user.UserName;
        Input.Email = user.Email;
        Input.Department = user.Department!;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var result = 
                from u in _userManager.Users
                where u.UserName == EmployeeName
                select u;
            var user = result.FirstOrDefault();
            if (user is null)
            {
                return RedirectToPage("/Index");
            }
            user!.Age = Input.Age;
            user!.Department = Input.Department;
            user!.Email = Input.Email;
            user!.UserName = Input.UserName;
            await _userManager.UpdateAsync(user!);
            return RedirectToPage("/Index");
        }
        return Page();
    }
}