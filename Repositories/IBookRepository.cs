using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Models;

namespace LibraryManagementEFCore.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllAsync();

    Task<Book?> GetByIdAsync(int id);

    Task<List<Book>> SearchAsync(string keyword);

    Task<bool> IsbnExistsAsync(
        string? isbn,
        int? excludeBookId = null);

    Task AddAsync(Book book);

    Task SaveChangesAsync();
}
