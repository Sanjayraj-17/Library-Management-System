
namespace LibraryManagementEFCore.Menus;

public class MainMenu
{
    private readonly DashboardMenu _dashboardMenu;
    private readonly BookMenu _bookMenu;
    private readonly MemberMenu _memberMenu;
    private readonly LoanMenu _loanMenu;

    public MainMenu(
        DashboardMenu dashboardMenu,
        BookMenu bookMenu,
        MemberMenu memberMenu,
        LoanMenu loanMenu)
    {
        _dashboardMenu = dashboardMenu;
        _bookMenu = bookMenu;
        _memberMenu = memberMenu;
        _loanMenu = loanMenu;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("===== LIBRARY MANAGEMENT SYSTEM =====");
            Console.WriteLine("1. Dashboard");
            Console.WriteLine("2. Book Management");
            Console.WriteLine("3. Member Management");
            Console.WriteLine("4. Loan Management");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await _dashboardMenu.RunAsync();
                    break;

                case "2":
                    await _bookMenu.RunAsync();
                    break;

                case "3":
                    await _memberMenu.RunAsync();
                    break;

                case "4":
                    await _loanMenu.RunAsync();
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


