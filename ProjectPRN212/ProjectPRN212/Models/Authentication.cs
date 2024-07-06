using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class Authentication
{
    public int Id { get; set; }

    public int? EmployeeId { get; set; }

    public string? Username { get; set; }

    public string? PassWord { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual Employee? Employee { get; set; }
}
