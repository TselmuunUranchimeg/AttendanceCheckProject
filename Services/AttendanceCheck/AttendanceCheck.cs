namespace Services.AttendanceCheck;

[Serializable]
public class CustomException: Exception
{
    public CustomException(string message): base(message) {}
}

public enum StatusEnum
{
    CheckIn, CheckOut
}

public class AttendanceCheck: IAttendanceCheck
{
    private readonly AttendanceDbContext _context;
    public AttendanceCheck(AttendanceDbContext context)
    {
        _context = context;
    }
    public Attendance Check(UserModel user, DateTime now, StatusEnum status)
    {
        TimeSpan jobStart = new DateTime(1, 1, 1, 7, 0, 0).TimeOfDay;
        TimeSpan lunchBreakBegin = new DateTime(1, 1, 1, 12, 0, 0).TimeOfDay;
        TimeSpan lunchBreakEnd = new DateTime(1, 1, 1, 13, 0, 0).TimeOfDay;
        TimeSpan jobEnd = new DateTime(1, 1, 1, 16, 0, 0).TimeOfDay;
        List<string> checkedAgainstMessages = new() {};
        if (status == StatusEnum.CheckIn)
        {
            checkedAgainstMessages.Add("Checked in");
            checkedAgainstMessages.Add("Checked in late");
        } else
        {
            checkedAgainstMessages.Add("Checked out");
            checkedAgainstMessages.Add("Checked out early");
        }
        if (!(now.TimeOfDay >= jobStart && now.TimeOfDay <= lunchBreakBegin) && !(now.TimeOfDay >= lunchBreakEnd && now.TimeOfDay <= jobEnd))
        {
            throw new CustomException("Not working hours!");
        }
        var latest = _context.Attendance?
            .Where(i => i.EmployeeId == user.Id && i.Date.Year == now.Year && i.Date.Month == now.Month && i.Date.Day == now.Day)
            .OrderByDescending(i => i.Date);
        if (latest is not null && latest.FirstOrDefault() is not null)
        {
            if (latest.Count() == 2)
            {
                throw new CustomException("Already done that!");
            }
            if (checkedAgainstMessages.Contains(latest.FirstOrDefault()!.Status))
            {
                throw new CustomException(String.Format(
                    status == StatusEnum.CheckIn 
                    ? "Can't check in without checking out first!" 
                    : "Can't check out without checking in first!"
                ));
            }
        }
        string message = String.Empty;
        if (status == StatusEnum.CheckIn)
        {
            message = "Checked in";
            if (now.TimeOfDay > new DateTime(1, 1, 1, 8, 30, 0).TimeOfDay)
            {
                message = "Checked in late";
            }
        } else if (status == StatusEnum.CheckOut)
        {
            message = "Checked out";
            if (now.TimeOfDay < new DateTime(1, 1, 1, 15, 0, 0).TimeOfDay)
            {
                message = "Checked out late";
            }
        }
        return new Attendance() {
            Status = message,
            EmployeeId = user.Id,
            Date = now
        };
    }
}