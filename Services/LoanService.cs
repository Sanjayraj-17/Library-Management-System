
using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Repositories;

namespace LibraryManagementEFCore.Services;

public class LoanService
{
    private readonly LoanRepository _loans;
    private readonly BookRepository _books;
    private readonly MemberRepository _members;

    // Fine per overdue day.
    private const decimal FinePerDay = 5m;

    public LoanService(
        LoanRepository loans,
        BookRepository books,
        MemberRepository members)
    {
        _loans = loans;
        _books = books;
        _members = members;
    }

    // Issue a book.
    public async Task<string> IssueBookAsync(
        int bookId,
        int memberId)
    {
        var book = await _books.GetByIdAsync(bookId);

        if (book == null)
            return "Book not found.";

        if (!book.IsActive)
            return "This book is inactive.";

        var member = await _members.GetByIdAsync(memberId);

        if (member == null)
            return "Member not found.";

        if (!member.IsActive)
            return "This member is inactive.";

        if (book.AvailableCopies <= 0)
            return "No copies available.";

        var loan = new Loan
        {
            BookId = bookId,
            MemberId = memberId,
            IssueDate = DateTime.Today,
            DueDate = DateTime.Today.AddDays(14),
            ReturnDate = null,
            FineAmount = 0
        };

        await _loans.IssueBookAsync(loan, book);

        return $"Book issued successfully! Loan ID: {loan.LoanId}";
    }

    // View loan history.
    public Task<List<Loan>> GetAllAsync()
    {
        return _loans.GetAllAsync();
    }

    // Return a book and calculate its fine.
    public async Task<string> ReturnBookAsync(int loanId)
    {
        var loan = await _loans.GetByIdAsync(loanId);

        if (loan == null)
            return "Loan record not found.";

        if (loan.ReturnDate.HasValue)
            return "This book has already been returned.";

        if (loan.Book == null)
            return "The associated book record was not found.";

        DateTime returnDate = DateTime.Today;

        int overdueDays = Math.Max(
            0,
            (returnDate.Date - loan.DueDate.Date).Days);

        decimal fine = overdueDays * FinePerDay;

        // Update the loan details.
        loan.ReturnDate = returnDate;
        loan.FineAmount = fine;

        // Save the loan and inventory changes.
        await _loans.ReturnBookAsync(loan, loan.Book);

        return overdueDays == 0
            ? "Book returned successfully. No fine."
            : $"Book returned successfully. Overdue days: {overdueDays}. Fine: {fine:C}";
    }
}
