using System;
using System.Collections.Generic;

namespace TaskManagementServiceBO;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusId { get; set; }

    public int CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual TaskCategory Category { get; set; } = null!;

    public virtual TaskStatus Status { get; set; } = null!;

    public virtual ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}
