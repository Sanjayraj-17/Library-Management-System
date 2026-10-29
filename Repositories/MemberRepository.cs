using System;
using System.Collections.Generic;
using System.Text;


using LibraryManagementEFCore.Data;
using LibraryManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Repositories;

public class MemberRepository : IMemberRepository
{
    private readonly AppDbContext _db;

    public MemberRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Member>> GetAllAsync()
    {
        return await _db.Members
            .AsNoTracking()
            .OrderBy(m => m.FullName)
            .ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _db.Members
            .FirstOrDefaultAsync(m => m.MemberId == id);
    }

    public async Task<bool> EmailExistsAsync(
        string email,
        int? excludeMemberId = null)
    {
        email = email.Trim();

        return await _db.Members.AnyAsync(m =>
            m.Email == email &&
            (!excludeMemberId.HasValue ||
             m.MemberId != excludeMemberId.Value));
    }

    public async Task AddAsync(Member member)
    {
        await _db.Members.AddAsync(member);
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
