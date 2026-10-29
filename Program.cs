
using LibraryManagementEFCore.Data;
using LibraryManagementEFCore.Menus;
using LibraryManagementEFCore.Repositories;
using LibraryManagementEFCore.Services;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var options =
            new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(AppDbContext.ConnectionString)
                .Options;

        await using var db = new AppDbContext(options);

        // Connect the repository to EF Core.
        var bookRepository = new BookRepository(db);

        // Connect the service to the repository.
        var bookService = new BookService(bookRepository);

        // Launch the book management menu.
        var bookMenu = new BookMenu(bookService);

        await bookMenu.RunAsync();
    }
}
