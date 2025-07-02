using System;
using System.Collections.Generic;

namespace Entity.Models;

public partial class Log
{
    public int LogId { get; set; }

    public string LogMessage { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
