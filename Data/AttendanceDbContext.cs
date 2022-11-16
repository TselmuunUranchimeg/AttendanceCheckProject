using Microsoft.EntityFrameworkCore;

namespace Data;

public class AttendanceDbContext: DbContext
{
    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options): base(options)
    {

    }
    public DbSet<Attendance>? Attendance { get; set; }
}