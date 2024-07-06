using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public decimal? Salary { get; set; }

    public int? RoleId { get; set; }

    public int? DepartmentId { get; set; }

    public int? ManagerId { get; set; }

    public int? PositionId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    public string? Address { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual ICollection<Authentication> Authentications { get; set; } = new List<Authentication>();

    public virtual Department? Department { get; set; }

    public virtual ICollection<EmployeeJob> EmployeeJobs { get; set; } = new List<EmployeeJob>();

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();

    public virtual Employee? Manager { get; set; }

    public virtual Position? Position { get; set; }

    public virtual Role? Role { get; set; }
}
