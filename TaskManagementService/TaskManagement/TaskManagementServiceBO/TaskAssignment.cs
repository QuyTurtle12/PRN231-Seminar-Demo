using System;
using System.Collections.Generic;

namespace TaskManagementServiceBO;

public partial class TaskAssignment
{
    public int TaskId { get; set; }

    public int UserId { get; set; }

    public DateTime AssignedDate { get; set; }

    public virtual Task Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
