using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class Leave
{
    public int LeaveId { get; set; }

    public int UserId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string Reason { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public int? ApprovedBy { get; set; }

    public string? Comment { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public decimal FromDateLeaveDuration { get; set; }

    public decimal ToDateLeaveDuration { get; set; }

    public decimal TotalLeave{ get; set; }

    public virtual User? ApprovedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
