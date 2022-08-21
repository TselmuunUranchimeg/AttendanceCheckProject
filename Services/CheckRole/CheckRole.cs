namespace Services.CheckRole;

public class CheckRole: ICheckRole
{
    private readonly UserManager<UserModel> _userManager;
    public CheckRole(UserManager<UserModel> userManager)
    {
        _userManager = userManager;
    }
    public async Task<bool> EmployeeIsAllowed(ClaimsPrincipal User, string allowedEmployeeId)
    {
        UserModel user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Employee"))
        {
            if (user.Id != allowedEmployeeId)
            {
                return false;
            }
        }
        return true;
    }
}