using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagementEFCore.Models;

public class AduitLogs
{
    public int AduitLogId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public int EntityId { get; set; }
    // Additional information about the action
    public string Description { get; set; } = string.Empty;

    // Who performed the action?
    public string PerformedBy { get; set; } = "System";

    // When did the action happen?
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;



}
