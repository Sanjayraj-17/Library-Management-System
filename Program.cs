
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

        // Book management dependencies
        var bookRepository = new BookRepository(db);
        var bookService = new BookService(bookRepository);
        var bookMenu = new BookMenu(bookService);

        // Member management dependencies
        var memberRepository = new MemberRepository(db);
        var memberService = new MemberService(memberRepository);
        var memberMenu = new MemberMenu(memberService);

        // Application main menu
        var mainMenu = new MainMenu(bookMenu, memberMenu);

        await mainMenu.RunAsync();
    }
}
