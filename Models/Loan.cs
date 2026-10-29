using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementEFCore.Models;

public class Loan
{
    // Primary key
    public int LoanId { get; set; }

    // Foreign keys
    public int BookId { get; set; }
    public int MemberId { get; set; }

    // Issue and return details
    public DateTime IssueDate { get; set; } = DateTime.Today;

    public DateTime DueDate { get; set; }
        = DateTime.Today.AddDays(14);

    // Null means the book has not been returned.
    public DateTime? ReturnDate { get; set; }

    // Fine amount
    public decimal FineAmount { get; set; }

    // Calculated property
    public bool IsReturned => ReturnDate.HasValue;

    // Navigation properties
    public Book? Book { get; set; }
    public Member? Member { get; set; }
}
