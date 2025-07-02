using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class User
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? ManagerId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? UserImageUrl { get; set; }

    public string? ResetPasswordCode { get; set; }

    public DateTime? ResetPasswordCodeExpiryTime { get; set; }

    public bool FirstLogin { get; set; }

    public int? DepartmentId { get; set; }
    public DateTime? RememberTime{ get; set; }

    public virtual ICollection<Attendance> AttendanceApprovedByNavigations { get; } = new List<Attendance>();

    public virtual ICollection<Attendance> AttendanceUsers { get; } = new List<Attendance>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<User> InverseManager { get; } = new List<User>();

    public virtual ICollection<Leave> LeaveApprovedByNavigations { get; } = new List<Leave>();

    public virtual ICollection<Leave> LeaveUsers { get; } = new List<Leave>();

    public virtual User? Manager { get; set; }

    public virtual Role Role { get; set; } = null!;
}
