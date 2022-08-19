using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class AttendanceDbContext: DbContext
{
    public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options): base(options)
    {

    }
    public DbSet<Attendance>? Attendance { get; set; }
}