using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementEFCore.Models;

public class Member
{
    // Primary key
    public int MemberId { get; set; }

    // Member information
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }

    // Registration information
    public DateTime JoinedOn { get; set; } = DateTime.Today;

    public bool IsActive { get; set; } = true;

    // Navigation property:
    // One member can have many loan records.
    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
}
