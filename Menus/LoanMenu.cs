
using LibraryManagementEFCore.Services;

namespace LibraryManagementEFCore.Menus;

public class LoanMenu
{
    private readonly LoanService _service;

    public LoanMenu(LoanService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== LOAN MANAGEMENT =====");
            Console.WriteLine("1. Issue Book");
            Console.WriteLine("2. View Loan History");
            Console.WriteLine("3. Return Book");
            Console.WriteLine("0. Back");
            Console.Write("Choose: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await IssueBookAsync();
                    break;

                case "2":
                    await ViewLoansAsync();
                    break;

                case "3":
                    await ReturnBookAsync();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    private async Task IssueBookAsync()
    {
        int bookId = ReadPositiveInt("Enter Book ID: ");
        int memberId = ReadPositiveInt("Enter Member ID: ");

        string result =
            await _service.IssueBookAsync(bookId, memberId);

        Console.WriteLine(result);
        Pause();
    }

    private async Task ReturnBookAsync()
    {
        Console.WriteLine("\n===== RETURN BOOK =====");

        int loanId = ReadPositiveInt("Enter Loan ID: ");

        string result =
            await _service.ReturnBookAsync(loanId);

        Console.WriteLine(result);
        Pause();
    }

    private async Task ViewLoansAsync()
    {
        var loans = await _service.GetAllAsync();

        Console.WriteLine("\n===== LOAN HISTORY =====");

        if (loans.Count == 0)
            Console.WriteLine("No loans found.");

        foreach (var loan in loans)
        {
            Console.WriteLine("------------------------");
            Console.WriteLine($"Loan ID: {loan.LoanId}");
            Console.WriteLine($"Book: {loan.Book?.Title ?? "Unknown"}");
            Console.WriteLine($"Member: {loan.Member?.FullName ?? "Unknown"}");
            Console.WriteLine($"Issue Date: {loan.IssueDate:yyyy-MM-dd}");
            Console.WriteLine($"Due Date: {loan.DueDate:yyyy-MM-dd}");
            Console.WriteLine(
                $"Return Date: {loan.ReturnDate?.ToString("yyyy-MM-dd") ?? "Not returned"}");
            Console.WriteLine($"Fine: {loan.FineAmount:C}");
            Console.WriteLine(
                $"Status: {(loan.ReturnDate.HasValue ? "Returned" : "Borrowed")}");
        }

        Pause();
    }

    private static int ReadPositiveInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int value)
                && value > 0)
            {
                return value;
            }

            Console.WriteLine("Enter a positive whole number.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
