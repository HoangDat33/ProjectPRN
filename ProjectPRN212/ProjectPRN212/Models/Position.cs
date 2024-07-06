using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class Position
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Salary { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
