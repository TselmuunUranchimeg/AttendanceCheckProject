namespace Services.AttendanceCheck;

public interface IAttendanceCheck
{
    Attendance Check(UserModel user, DateTime now, StatusEnum status);
}