using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Repositories;

namespace LibraryManagementEFCore.Services;

public class MemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository)
    {
        _repository = repository;
    }

    public Task<List<Member>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public Task<Member?> GetByIdAsync(int id)
    {
        return _repository.GetByIdAsync(id);
    }

    public async Task<string> RegisterAsync(Member member)
    {
        if (string.IsNullOrWhiteSpace(member.FullName))
            return "Member name is required.";

        if (string.IsNullOrWhiteSpace(member.Email))
            return "Email is required.";

        member.FullName = member.FullName.Trim();
        member.Email = member.Email.Trim().ToLowerInvariant();

        if (!IsValidEmail(member.Email))
            return "Please enter a valid email address.";

        if (member.Phone is not null)
            member.Phone = member.Phone.Trim();

        if (string.IsNullOrWhiteSpace(member.Phone))
            member.Phone = null;

        if (await _repository.EmailExistsAsync(member.Email))
            return "This email is already registered.";

        member.JoinedOn = DateTime.Today;
        member.IsActive = true;

        await _repository.AddAsync(member);
        await _repository.SaveChangesAsync();

        return $"Member registered successfully. Member ID: {member.MemberId}";
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var parsed = new System.Net.Mail.MailAddress(email);

            return parsed.Address.Equals(
                email,
                StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
