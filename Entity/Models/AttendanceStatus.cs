using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class AttendanceStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;
}
