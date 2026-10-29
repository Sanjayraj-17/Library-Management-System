using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Services;

namespace LibraryManagementEFCore.Menus;

public class BookMenu
{
    private readonly BookService _service;

    public BookMenu(BookService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("================================");
            Console.WriteLine("       BOOK MANAGEMENT");
            Console.WriteLine("================================");
            Console.WriteLine("1. List Books");
            Console.WriteLine("2. Add Book");
            Console.WriteLine("3. Search Books");
            Console.WriteLine("4. Update Book");
            Console.WriteLine("5. Deactivate Book");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await ListBooksAsync();
                    break;
                case "2":
                    await AddBookAsync();
                    break;
                case "3":
                    await SearchBooksAsync();
                    break;
                case "4":
                    await UpdateBookAsync();
                    break;
                case "5":
                    await DeactivateBookAsync();
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

    private async Task ListBooksAsync()
    {
        Console.Clear();
        var books = await _service.GetAllAsync();

        Console.WriteLine("--------------- BOOK LIST ---------------");

        if (books.Count == 0)
        {
            Console.WriteLine("No active books found.");
        }

        foreach (var book in books)
        {
            PrintBook(book);
        }

        Pause();
    }

    private async Task AddBookAsync()
    {
        Console.Clear();
        Console.WriteLine("--------------- ADD BOOK ---------------");

        var book = new Book
        {
            Title = ReadRequired("Title: "),
            Author = ReadRequired("Author: "),
            ISBN = ReadOptional("ISBN (optional): "),
            Category = ReadOptional("Category (optional): "),
            Price = ReadDecimal("Price: "),
            TotalCopies = ReadInt("Total copies: ")
        };

        Console.WriteLine(await _service.AddAsync(book));
        Pause();
    }

    private async Task SearchBooksAsync()
    {
        Console.Clear();
        string keyword = ReadRequired("Enter title, author, or ISBN: ");

        var books = await _service.SearchAsync(keyword);

        if (books.Count == 0)
        {
            Console.WriteLine("No matching active books found.");
        }

        foreach (var book in books)
        {
            PrintBook(book);
        }

        Pause();
    }

    private async Task UpdateBookAsync()
    {
        Console.Clear();

        int id = ReadInt("Book ID to update: ");
        var book = await _service.GetByIdAsync(id);

        if (book is null || !book.IsActive)
        {
            Console.WriteLine("Active book not found.");
            Pause();
            return;
        }

        Console.WriteLine($"Current title: {book.Title}");
        Console.WriteLine("Enter new details.");

        string title = ReadRequired("New title: ");
        string author = ReadRequired("New author: ");
        string? isbn = ReadOptional("New ISBN (blank to clear): ");
        string? category = ReadOptional("New category (blank to clear): ");
        decimal price = ReadDecimal("New price: ");
        int copies = ReadInt("New total copies: ");

        Console.WriteLine(
            await _service.UpdateAsync(
                id, title, author, isbn, category, price, copies));

        Pause();
    }

    private async Task DeactivateBookAsync()
    {
        Console.Clear();

        int id = ReadInt("Book ID to deactivate: ");

        Console.Write(
            "Are you sure you want to deactivate this book? (y/n): ");

        if (!string.Equals(
                Console.ReadLine()?.Trim(),
                "y",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Operation cancelled.");
            Pause();
            return;
        }

        Console.WriteLine(await _service.DeactivateAsync(id));
        Pause();
    }

    private static void PrintBook(Book book)
    {
        Console.WriteLine("------------------------------------------");
        Console.WriteLine($"ID: {book.BookId}");
        Console.WriteLine($"Title: {book.Title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"ISBN: {book.ISBN ?? "N/A"}");
        Console.WriteLine($"Category: {book.Category ?? "N/A"}");
        Console.WriteLine($"Price: {book.Price:C}");
        Console.WriteLine(
            $"Available: {book.AvailableCopies}/{book.TotalCopies}");
        Console.WriteLine($"Status: {(book.IsActive ? "Active" : "Inactive")}");
    }

    private static string ReadRequired(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(value))
                return value;

            Console.WriteLine("This field is required.");
        }
    }

    private static string? ReadOptional(string prompt)
    {
        Console.Write(prompt);
        string? value = Console.ReadLine()?.Trim();

        return string.IsNullOrWhiteSpace(value) ? null : value;
    }

    private static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.WriteLine("Enter a valid whole number.");
        }
    }

    private static decimal ReadDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            if (decimal.TryParse(Console.ReadLine(), out decimal value)
                && value >= 0)
                return value;

            Console.WriteLine("Enter a valid non-negative amount.");
        }
    }

    private static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
