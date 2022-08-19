namespace AttendanceCheckProject.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<UserModel> _userManager;

    public IndexModel(ILogger<IndexModel> logger, UserManager<UserModel> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        UserModel user = await _userManager.GetUserAsync(User);
        if (user is not null)
        {
            return RedirectToPage("/Homepage");
        }
        return Page();
    }
}
