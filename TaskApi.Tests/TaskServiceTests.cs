using Microsoft.EntityFrameworkCore;
using TaskApi.Data;
using TaskApi.DTOs;
using TaskApi.Models;
using TaskApi.Services;
using Xunit;

namespace TaskApi.Tests;

public class TaskServiceTests
{
    [Fact]
    public async Task CreateAsync_AddsTaskAndReturnsDto()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var db = new AppDbContext(options);
        var service = new TaskService(db);

        var createDto = new CreateTaskDto
        {
            Title = "Test task",
            Description = "Validate service create",
            Priority = Priority.Low,
            DueDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await service.CreateAsync(createDto);

        Assert.NotNull(result);
        Assert.Equal(createDto.Title, result.Title);
        Assert.Equal("Low", result.Priority);
        Assert.False(result.IsCompleted);
        Assert.True(result.Id > 0);

        var persisted = await db.Tasks.FindAsync(result.Id);
        Assert.NotNull(persisted);
        Assert.Equal(createDto.Description, persisted!.Description);
    }
}