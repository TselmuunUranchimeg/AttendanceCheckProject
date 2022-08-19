namespace Models;

public class Attendance
{
    public int ID { get; set; }
    public string EmployeeId { get; set; } = "";
    public string Status { get; set; } = "";

    [DataType(DataType.DateTime)]
    public DateTime Date { get; set; }

    public string? Reason { get; set; }
}