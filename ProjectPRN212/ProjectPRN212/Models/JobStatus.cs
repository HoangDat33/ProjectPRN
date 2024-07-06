using System;
using System.Collections.Generic;

namespace ProjectPRN212.Models;

public partial class JobStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public DateOnly? DeletedAt { get; set; }

    public bool? IsDelete { get; set; }

    public int? DeletedById { get; set; }

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
