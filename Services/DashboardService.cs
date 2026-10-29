using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Services;

public class DashboardService
{
    private readonly AppDbContext _db;

    public DashboardService(AppDbContext db)
    {
        _db = db;
    }

    public async Task ShowDashboardAsync()
    {
        // Count active book titles.
        int totalBooks = await _db.Books
            .CountAsync(b => b.IsActive);

        // Count active members.
        int totalMembers = await _db.Members
            .CountAsync(m => m.IsActive);

        // Count available copies of active books.
        int availableCopies = await _db.Books
            .Where(b => b.IsActive)
            .SumAsync(b => (int?)b.AvailableCopies) ?? 0;

        // Count loans that have not been returned.
        int activeLoans = await _db.Loans
            .CountAsync(l => l.ReturnDate == null);

        Console.Clear();

        Console.WriteLine("==================================");
        Console.WriteLine("       LIBRARY DASHBOARD");
        Console.WriteLine("==================================");
        Console.WriteLine($"Total book titles : {totalBooks}");
        Console.WriteLine($"Total members     : {totalMembers}");
        Console.WriteLine($"Available copies  : {availableCopies}");
        Console.WriteLine($"Active loans      : {activeLoans}");
        Console.WriteLine("==================================");
        Console.WriteLine("\nPress Enter to return to the menu.");
        Console.ReadLine();
    }
}
