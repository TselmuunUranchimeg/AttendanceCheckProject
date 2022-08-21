[ApiController]
[Route("/AttendanceData")]
public class FetchAttendance: ControllerBase
{
    private readonly AttendanceDbContext _context;
    public FetchAttendance(AttendanceDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{employeeId}/{year}/{month}")]
    public ActionResult<Attendance> FetchData(string employeeId, int year, int month)
    {
        var data = from a in _context.Attendance 
        where a.EmployeeId == employeeId && a.Date.Year == year && a.Date.Month == month
        select a;
        return Ok(data);
    }
}