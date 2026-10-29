using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Repositories;

namespace LibraryManagementEFCore.Services;

public class BookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Book>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public Task<List<Book>> SearchAsync(string keyword)
    {
        return _repository.SearchAsync(keyword);
    }

    public async Task<string> AddAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
            return "Title is required.";

        if (string.IsNullOrWhiteSpace(book.Author))
            return "Author is required.";

        if (book.Price < 0)
            return "Price cannot be negative.";

        if (book.TotalCopies < 1)
            return "Total copies must be at least 1.";

        if (string.IsNullOrWhiteSpace(book.ISBN))
        {
            book.ISBN = null;
        }
        else
        {
            book.ISBN = book.ISBN.Trim();

            if (await _repository.IsbnExistsAsync(book.ISBN))
                return "This ISBN already exists.";
        }

        book.Title = book.Title.Trim();
        book.Author = book.Author.Trim();
        book.Category = string.IsNullOrWhiteSpace(book.Category)
            ? null
            : book.Category.Trim();

        book.AvailableCopies = book.TotalCopies;
        book.IsActive = true;

        await _repository.AddAsync(book);
        await _repository.SaveChangesAsync();

        return "Book added successfully.";
    }

    public async Task<string> UpdateAsync(
        int id,
        string title,
        string author,
        string? isbn,
        string? category,
        decimal price,
        int totalCopies)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book is null)
            return "Book not found.";

        if (!book.IsActive)
            return "Cannot update an inactive book.";

        if (string.IsNullOrWhiteSpace(title))
            return "Title is required.";

        if (string.IsNullOrWhiteSpace(author))
            return "Author is required.";

        if (price < 0)
            return "Price cannot be negative.";

        if (totalCopies < 1)
            return "Total copies must be at least 1.";

        if (string.IsNullOrWhiteSpace(isbn))
        {
            isbn = null;
        }
        else
        {
            isbn = isbn.Trim();

            if (await _repository.IsbnExistsAsync(isbn, id))
                return "This ISBN belongs to another book.";
        }

        // Copies already issued must remain accounted for.
        int issuedCopies = book.TotalCopies - book.AvailableCopies;

        if (totalCopies < issuedCopies)
            return $"Cannot set total copies below {issuedCopies}, because those copies are issued.";

        book.Title = title.Trim();
        book.Author = author.Trim();
        book.ISBN = isbn;
        book.Category = string.IsNullOrWhiteSpace(category)
            ? null
            : category.Trim();
        book.Price = price;

        book.TotalCopies = totalCopies;
        book.AvailableCopies = totalCopies - issuedCopies;

        await _repository.SaveChangesAsync();

        return "Book updated successfully.";
    }

    public async Task<string> DeactivateAsync(int id)
    {
        var book = await _repository.GetByIdAsync(id);

        if (book is null)
            return "Book not found.";

        if (!book.IsActive)
            return "Book is already inactive.";

        if (book.AvailableCopies != book.TotalCopies)
            return "Cannot deactivate this book while copies are issued.";

        book.IsActive = false;

        await _repository.SaveChangesAsync();

        return "Book deactivated successfully.";
    }
}
