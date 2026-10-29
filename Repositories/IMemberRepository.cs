using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Models;

namespace LibraryManagementEFCore.Repositories;

public interface IMemberRepository
{
    Task<List<Member>> GetAllAsync();

    Task<Member?> GetByIdAsync(int id);

    Task<bool> EmailExistsAsync(
        string email,
        int? excludeMemberId = null);

    Task AddAsync(Member member);

    Task SaveChangesAsync();
}

