using System;
using System.Collections.Generic;
using System.Text;


namespace LibraryManagementEFCore.Menus;

public class MainMenu
{
    private readonly BookMenu _bookMenu;
    private readonly MemberMenu _memberMenu;

    public MainMenu(
        BookMenu bookMenu,
        MemberMenu memberMenu)
    {
        _bookMenu = bookMenu;
        _memberMenu = memberMenu;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("   LIBRARY MANAGEMENT SYSTEM");
            Console.WriteLine("================================");
            Console.WriteLine("1. Book Management");
            Console.WriteLine("2. Member Management");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await _bookMenu.RunAsync();
                    break;

                case "2":
                    await _memberMenu.RunAsync();
                    break;

                case "0":
                    Console.WriteLine("Exiting application...");
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }
}
