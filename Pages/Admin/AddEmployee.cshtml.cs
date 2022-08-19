using Microsoft.Extensions.Logging;

namespace AttendanceCheckProject.Pages.Admin;

[Authorize(Roles = "Admin")]
public class AddEmployeeModel: PageModel
{
    public class InputModel
    {
        [Required]
        public int Age { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Department { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    private readonly UserManager<UserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AddEmployeeModel> _logger;
    private readonly IUserStore<UserModel> _userStore;

    public AddEmployeeModel(
        UserManager<UserModel> userManager, 
        RoleManager<IdentityRole> roleManager,
        ILogger<AddEmployeeModel> logger,
        IUserStore<UserModel> userStore)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _userStore = userStore;
    }

    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (ModelState.IsValid)
            {
                UserModel user = new()
                {
                    Age = Input.Age,
                    Email = Input.Email,
                    UserName = Input.Name,
                    Department = Input.Department
                };
                /* await _userStore.SetUserNameAsync(user, user.UserName, CancellationToken.None); */
                IdentityResult result = await _userManager.CreateAsync(user, Input.Password);
                if (!await _roleManager.RoleExistsAsync("Employee"))
                {
                    await _roleManager.CreateAsync(new IdentityRole() { Name = "Employee" });
                }
                await _userManager.AddToRoleAsync(user, "Employee");
                if (result.Succeeded)
                {
                    return RedirectToPage("/Admin/Homepage");
                }
            }
            return Page();
        }
        catch (Exception e)
        {
            _logger.LogInformation(e.Message);
            return Page();
        }
    }
}