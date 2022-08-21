namespace AttendanceCheckProject.Pages.Employee;

[Authorize(Roles = "Admin")]
public class EmployeeDeleteModel: PageModel
{
    private readonly UserManager<UserModel> _userManager;

    [BindProperty]
    public UserModel UserData { get; set; } = new();

    public EmployeeDeleteModel(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty(SupportsGet = true)]
    public string Name { get; set; } = "";
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.FindByNameAsync(Name);
        if (user is null)
        {
            return NotFound();
        }
        UserData = user;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        UserModel user = await _userManager.FindByNameAsync(Name);
        if (user is null)
        {
            return NotFound();
        }
        if ((await _userManager.GetUserAsync(User)).Id == user.Id)
        {
            ViewData["ErrorMessage"] = "Can't delete your own account!";
            return await OnGetAsync();
        }
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToPage("/Index");
        }
        return Page();
    }
}