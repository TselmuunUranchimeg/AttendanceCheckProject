namespace AttendanceCheckProject.Pages;

public class LogoutModel: PageModel
{
    private readonly SignInManager<UserModel> _signInManager;
    public LogoutModel(SignInManager<UserModel> signInManager)
    {
        _signInManager = signInManager;
    }
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        await _signInManager.SignOutAsync();
        return RedirectToPage("/Index");
    }
}