using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectPRN212.Models;

public partial class ProjectPrn212Context : DbContext
{
    public static ProjectPrn212Context INSTANCE = new ProjectPrn212Context();

    public ProjectPrn212Context()
    {
    }

    public ProjectPrn212Context(DbContextOptions<ProjectPrn212Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Authentication> Authentications { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeJob> EmployeeJobs { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobStatus> JobStatuses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        var config = new ConfigurationBuilder().AddJsonFile("appseting.json").Build();
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(config.GetConnectionString("DBContext"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Authentication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authenti__3214EC27D8F88961");

            entity.ToTable("Authentication");

            entity.HasIndex(e => e.Username, "UQ_Authentication_Username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.PassWord)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(40)
                .IsUnicode(false);

            entity.HasOne(d => d.Employee).WithMany(p => p.Authentications)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Authentic__Emplo__4BAC3F29");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC27BB4EB635");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(55);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(55);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC2794D12B79");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Email)
                .HasMaxLength(55)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(55);
            entity.Property(e => e.Gender).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(55);
            entity.Property(e => e.ManagerId).HasColumnName("ManagerID");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PositionId).HasColumnName("PositionID");
            entity.Property(e => e.RoleId)
                .HasDefaultValue(2)
                .HasColumnName("RoleID");
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__Depar__44FF419A");

            entity.HasOne(d => d.Manager).WithMany(p => p.InverseManager)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Employees__Manag__45F365D3");

            entity.HasOne(d => d.Position).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__Posit__440B1D61");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Employees__RoleI__4316F928");
        });

        modelBuilder.Entity<EmployeeJob>(entity =>
        {
            entity.HasKey(e => e.EmployeeJobId).HasName("PK__Employee__F7369751DC5D3BC2");

            entity.HasIndex(e => new { e.EmployeeId, e.JobId }, "UQ_EmployeeJobs").IsUnique();

            entity.Property(e => e.EmployeeJobId).HasColumnName("EmployeeJobID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.JobId).HasColumnName("JobID");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeJobs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeJ__Emplo__59063A47");

            entity.HasOne(d => d.Job).WithMany(p => p.EmployeeJobs)
                .HasForeignKey(d => d.JobId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__EmployeeJ__JobID__59FA5E80");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jobs__3214EC27BBE51AA2");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK__Jobs__AssignedBy__534D60F1");

            entity.HasOne(d => d.Department).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Jobs__Department__5441852A");

            entity.HasOne(d => d.JobStatus).WithMany(p => p.Jobs)
                .HasForeignKey(d => d.JobStatusId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Jobs__JobStatusI__52593CB8");
        });

        modelBuilder.Entity<JobStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JobStatu__3214EC2715311778");

            entity.ToTable("JobStatus");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC27C43B4DFB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeletedById).HasColumnName("DeletedByID");
            entity.Property(e => e.Description).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(55);
            entity.Property(e => e.Salary).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC274BE35675");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DeleteById).HasColumnName("DeleteByID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.RoleName).HasMaxLength(55);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
