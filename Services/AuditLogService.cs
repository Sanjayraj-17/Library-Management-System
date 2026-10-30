using System;
using System.Collections.Generic;
using System.Text;

using LibraryManagementEFCore.Models;
using LibraryManagementEFCore.Repositories;

namespace LibraryManagementEFCore.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    // Record an action in the audit log
    public async Task LogActionAsync(
        string action,
        string entityName,
        int? entityId,
        string description,
        string performedBy = "System")
    {
        var auditLog = new AuditLog
        {
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            Description = description,
            PerformedBy = performedBy,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(auditLog);
    }

    // Retrieve all audit records
    public async Task<List<AuditLog>> GetAllLogsAsync()
    {
        return await _repository.GetAllAsync();
    }

    // Retrieve a particular audit record
    public async Task<AuditLog?> GetLogByIdAsync(int id)
    {
        if (id <= 0)
        {
            return null;
        }

        return await _repository.GetByIdAsync(id);
    }
}