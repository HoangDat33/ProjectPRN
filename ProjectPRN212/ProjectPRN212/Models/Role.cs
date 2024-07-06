using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeleteById { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
