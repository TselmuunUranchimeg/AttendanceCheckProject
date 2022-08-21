namespace Services.CheckRole;

public interface ICheckRole
{
    Task<bool> EmployeeIsAllowed(ClaimsPrincipal User, string allowedEmployeeId);
}