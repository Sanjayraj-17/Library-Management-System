
using LibraryManagementEFCore.Data;
using LibraryManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Repositories;

public class LoanRepository
{
    private readonly AppDbContext _db;

    public LoanRepository(AppDbContext db)
    {
        _db = db;
    }

    // View all loan records.
    public async Task<List<Loan>> GetAllAsync()
    {
        return await _db.Loans
            .AsNoTracking()
            .Include(l => l.Book)
            .Include(l => l.Member)
            .OrderByDescending(l => l.IssueDate)
            .ToListAsync();
    }

    // Find one loan by ID.
    public async Task<Loan?> GetByIdAsync(int id)
    {
        return await _db.Loans
            .Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.LoanId == id);
    }

    // Issue a book and reduce available copies.
    public async Task IssueBookAsync(Loan loan, Book book)
    {
        await using var transaction =
            await _db.Database.BeginTransactionAsync();

        try
        {
            book.AvailableCopies--;

            _db.Loans.Add(loan);

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // Return a book and restore available copies.
    public async Task ReturnBookAsync(Loan loan, Book book)
    {
        await using var transaction =
            await _db.Database.BeginTransactionAsync();

        try
        {
            book.AvailableCopies++;

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
