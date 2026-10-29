using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Data;
using LibraryManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;

    public BookRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Book>> GetAllAsync()
    {
        return await _db.Books
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _db.Books
            .FirstOrDefaultAsync(b => b.BookId == id);
    }

    public async Task<List<Book>> SearchAsync(string keyword)
    {
        keyword = keyword.Trim();

        return await _db.Books
            .AsNoTracking()
            .Where(b => b.IsActive &&
                (b.Title.Contains(keyword) ||
                 b.Author.Contains(keyword) ||
                 (b.ISBN != null && b.ISBN.Contains(keyword))))
            .OrderBy(b => b.Title)
            .ToListAsync();
    }

    public async Task<bool> IsbnExistsAsync(
        string? isbn,
        int? excludeBookId = null)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            return false;

        isbn = isbn.Trim();

        return await _db.Books.AnyAsync(b =>
            b.ISBN == isbn &&
            (!excludeBookId.HasValue ||
             b.BookId != excludeBookId.Value));
    }

    public async Task AddAsync(Book book)
    {
        await _db.Books.AddAsync(book);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}

