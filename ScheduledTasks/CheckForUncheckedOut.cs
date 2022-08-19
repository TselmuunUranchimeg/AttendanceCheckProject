using Quartz;

namespace ScheduledTasks;

public class CheckForUncheckedOut: IJob
{
    private readonly UserManager<UserModel> _userManager;
    private readonly AttendanceDbContext _context;

    public CheckForUncheckedOut(
        UserManager<UserModel> userManager, AttendanceDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    ///<summary>
    ///Check for users who already checked out from 3 till 4, and find the excluded ones
    ///Check them out at 4 while still checking for their last activity 
    ///</summary>
    public async Task Execute(IJobExecutionContext context)
    {
        DateTime thatDay = DateTime.Now;
        var alreadyCheckedOutUserIds = 
            from a in _context.Attendance!.AsEnumerable()
            where a.Date.Day == thatDay.Day
            where a.Date.TimeOfDay <= thatDay.TimeOfDay
            where a.Date.TimeOfDay >= new DateTime(1, 1, 1, 15, 0, 0).TimeOfDay
            where a.Status == "Checked out" || a.Status == "Checked out early"
            select a.EmployeeId;
        if (alreadyCheckedOutUserIds is null)
        {
            await Task.WhenAll(_userManager.Users.AsEnumerable().Select<UserModel, Task>(
                async (UserModel user) => await CheckAndAdd(user, _context, thatDay)
            ));
            return;
        }
        var notCheckedOutUsers = _userManager.Users.Where(
            i => !alreadyCheckedOutUserIds!.Contains(i.Id)).AsEnumerable();
        await Task.WhenAll(notCheckedOutUsers.Select<UserModel, Task>(async (UserModel user) =>
            await CheckAndAdd(user, _context, thatDay)));
        await Task.CompletedTask;
    }

    public async Task CheckAndAdd(UserModel user, AttendanceDbContext _context, DateTime thatDay)
    {
        var latestActivity = _context.Attendance!
            .OrderByDescending(i => i.Date)
            .Where(i => i.EmployeeId == user.Id)
            .FirstOrDefault();
        if (latestActivity?.Status == "Checked in" || latestActivity?.Status == "Checked in late")
        {
            _context.Attendance?.Add(new Attendance() {
                Status = "Checked out",
                EmployeeId = user.Id,
                Date = thatDay
            });
            await _context.SaveChangesAsync();
        }
    }
}