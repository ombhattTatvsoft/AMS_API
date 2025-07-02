using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly? InTime { get; set; }

    public TimeOnly? OutTime { get; set; }

    public int StatusId { get; set; }

    public bool? IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public int? LeaveId { get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
