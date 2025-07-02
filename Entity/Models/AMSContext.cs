using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Entity.Models;

public partial class AMSContext : DbContext
{
    public AMSContext()
    {
    }

    public AMSContext(DbContextOptions<AMSContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<AttendanceStatus> AttendanceStatuses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Holiday> Holidays { get; set; }

    public virtual DbSet<Leave> Leaves { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("attendances_pkey");

            entity.ToTable("attendances");

            entity.Property(e => e.AttendanceId)
                .HasDefaultValueSql("nextval('attendances_attendanceid_seq'::regclass)")
                .HasColumnName("attendance_id");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.InTime).HasColumnName("in_time");
            entity.Property(e => e.IsApproved).HasColumnName("is_approved");
            entity.Property(e => e.OutTime).HasColumnName("out_time");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LeaveId).HasColumnName("leave_id");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.AttendanceApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("attendances_approvedby_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.AttendanceUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("attendances_userid_fkey");
        });

        modelBuilder.Entity<AttendanceStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("attendance_status_pkey");

            entity.ToTable("attendance_status");

            entity.HasIndex(e => e.StatusName, "attendance_status_status_name_key").IsUnique();

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName).HasColumnName("status_name");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("departments_pkey");

            entity.ToTable("departments");

            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("department_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<Holiday>(entity =>
        {
            entity.HasKey(e => e.HolidayId).HasName("holiday_pkey");

            entity.ToTable("holiday");

            entity.Property(e => e.HolidayId)
                .HasDefaultValueSql("nextval('holiday_holidayid_seq'::regclass)")
                .HasColumnName("holiday_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.HolidayDate).HasColumnName("holiday_date");
            entity.Property(e => e.HolidayName)
                .HasMaxLength(100)
                .HasColumnName("holiday_name");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
        });

        modelBuilder.Entity<Leave>(entity =>
        {
            entity.HasKey(e => e.LeaveId).HasName("leaves_pkey");

            entity.ToTable("leaves");

            entity.Property(e => e.LeaveId)
                .HasDefaultValueSql("nextval('leaves_leaveid_seq'::regclass)")
                .HasColumnName("leave_id");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FromDate).HasColumnName("from_date");
            entity.Property(e => e.IsApproved).HasColumnName("is_approved");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.FromDateLeaveDuration)
                .HasPrecision(2, 1)
                .HasColumnName("from_date_leave_duration");
            entity.Property(e => e.ToDateLeaveDuration)
                .HasPrecision(2, 1)
                .HasColumnName("to_date_leave_duration");
            entity.Property(e => e.TotalLeave)
                .HasColumnName("total_leave");
            entity.Property(e => e.Reason)
                .HasMaxLength(500)
                .HasColumnName("reason");
            entity.Property(e => e.ToDate).HasColumnName("to_date");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.LeaveApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("leaves_approvedby_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.LeaveUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leaves_userid_fkey");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.Property(e => e.LogId)
                .HasDefaultValueSql("nextval('logs_logid_seq'::regclass)")
                .HasColumnName("log_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LogMessage)
                .HasMaxLength(1000)
                .HasColumnName("log_message");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_rolename_key").IsUnique();

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("nextval('roles_roleid_seq'::regclass)")
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("nextval('users_userid_seq'::regclass)")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstLogin)
                .IsRequired()
                .HasDefaultValueSql("true")
                .HasColumnName("first_login");
            entity.Property(e => e.RememberTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("remember_time");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.ManagerId).HasColumnName("manager_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .HasColumnName("password");
            entity.Property(e => e.ResetPasswordCode).HasMaxLength(100);
            entity.Property(e => e.ResetPasswordCodeExpiryTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ResetPasswordCodeExpiryTime");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UserImageUrl).HasColumnName("user_image_url");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("users_department_id_fkey");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("users_managerid_fkey");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_roleid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
