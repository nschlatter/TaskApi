using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Models;

namespace TaskApi.Services;

public interface ITaskService
{
    Task<PagedResponse<TaskResponseDto>> GetAllAsync(int page, int pageSize, bool? isCompleted, Priority? priority);
    Task<TaskResponseDto?> GetByIdAsync(int id);
    Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
    Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto);
    Task<bool> DeleteAsync(int id);
    Task<TaskResponseDto?> ToggleCompleteAsync(int id);
}

public class TaskService(AppDbContext db) : ITaskService
{
    public async Task<PagedResponse<TaskResponseDto>> GetAllAsync(
        int page, int pageSize, bool? isCompleted, Priority? priority)
    {
        var query = db.Tasks.AsQueryable();

        if (isCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == isCompleted.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => MapToDto(t))
            .ToListAsync();

        return new PagedResponse<TaskResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);
        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            CreatedAt = DateTime.UtcNow
        };

        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        return MapToDto(task);
    }

    public async Task<TaskResponseDto?> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;

        await db.SaveChangesAsync();
        return MapToDto(task);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return false;

        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<TaskResponseDto?> ToggleCompleteAsync(int id)
    {
        var task = await db.Tasks.FindAsync(id);
        if (task is null) return null;

        task.IsCompleted = !task.IsCompleted;
        await db.SaveChangesAsync();
        return MapToDto(task);
    }

    private static TaskResponseDto MapToDto(TaskItem task) => new()
    {
        Id = task.Id,
        Title = task.Title,
        Description = task.Description,
        IsCompleted = task.IsCompleted,
        Priority = task.Priority.ToString(),
        CreatedAt = task.CreatedAt,
        DueDate = task.DueDate
    };
}
