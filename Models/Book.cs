using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementEFCore.Models;

public class Book
{
    // Primary key
    public int BookId { get; set; }

    // Book details
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public string? Category { get; set; }

    // Inventory and price
    public decimal Price { get; set; }
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;

    // Soft-delete flag
    public bool IsActive { get; set; } = true;

    // Navigation property:
    // One book can have many loan records.
    public ICollection<Loan> Loans { get; set; }
        = new List<Loan>();
}
