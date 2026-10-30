using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementEFCore.Data;
using LibraryManagementEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementEFCore.Repositories;

public class AuditLogRepository
{
    private readonly AppDbContext _db;

    public AuditLogRepository(AppDbContext db)
    {
        _db = db;
    }

    // Add a new audit log
    public async Task AddAsync(AuditLog auditLog)
    {
        await _db.AuditLogs.AddAsync(auditLog);

        await _db.SaveChangesAsync();
    }

    // Get all audit logs
    public async Task<List<AuditLog>> GetAllAsync()
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    // Get audit log by ID
    public async Task<AuditLog?> GetByIdAsync(int id)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AuditLogId == id);
    }
}