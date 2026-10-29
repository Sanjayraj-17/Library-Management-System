using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Services;

namespace LibraryManagementEFCore.Menus;

public class MemberMenu
{
    private readonly MemberService _service;

    public MemberMenu(MemberService service)
    {
        _service = service;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("================================");
            Console.WriteLine("      MEMBER MANAGEMENT");
            Console.WriteLine("================================");
            Console.WriteLine("1. List Members");
            Console.WriteLine("2. Register Member");
            Console.WriteLine("0. Back");
            Console.Write("Choose an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    await ListMembersAsync();
                    break;

                case "2":
                    await RegisterMemberAsync();
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

    private async Task ListMembersAsync()
    {
        Console.Clear();

        var members = await _service.GetAllAsync();

        Console.WriteLine("-------------- MEMBER LIST --------------");

        if (members.Count == 0)
        {
            Console.WriteLine("No members registered.");
        }

        foreach (var member in members)
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine($"Member ID : {member.MemberId}");
            Console.WriteLine($"Name      : {member.FullName}");
            Console.WriteLine($"Email     : {member.Email}");
            Console.WriteLine($"Phone     : {member.Phone ?? "N/A"}");
            Console.WriteLine($"Joined    : {member.JoinedOn:yyyy-MM-dd}");
            Console.WriteLine(
                $"Status    : {(member.IsActive ? "Active" : "Inactive")}");
        }

        Pause();
    }

    private async Task RegisterMemberAsync()
    {
        Console.Clear();

        Console.WriteLine("------------ REGISTER MEMBER ------------");

        var member = new Member
        {
            FullName = ReadRequired("Full name: "),
            Email = ReadRequired("Email address: "),
            Phone = ReadOptional("Phone number (optional): ")
        };

        string result = await _service.RegisterAsync(member);

        Console.WriteLine();
        Console.WriteLine(result);

        Pause();
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

    private static void Pause()
    {
        Console.WriteLine("\nPress Enter to continue...");
        Console.ReadLine();
    }
}
