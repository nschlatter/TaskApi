using Microsoft.AspNetCore.Mvc;
using TaskApi.DTOs;
using TaskApi.Models;
using TaskApi.Services;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    /// <summary>Get all tasks with optional filtering and pagination</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<TaskResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isCompleted = null,
        [FromQuery] Priority? priority = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 100)
            return BadRequest("Invalid pagination parameters.");

        var result = await taskService.GetAllAsync(page, pageSize, isCompleted, priority);
        return Ok(result);
    }

    /// <summary>Get a task by ID</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await taskService.GetByIdAsync(id);
        return task is null ? NotFound(new { message = $"Task {id} not found." }) : Ok(task);
    }

    /// <summary>Create a new task</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var task = await taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>Update an existing task</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTaskDto dto)
    {
        var task = await taskService.UpdateAsync(id, dto);
        return task is null ? NotFound(new { message = $"Task {id} not found." }) : Ok(task);
    }

    /// <summary>Delete a task</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await taskService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound(new { message = $"Task {id} not found." });
    }

    /// <summary>Toggle a task's completion status</summary>
    [HttpPatch("{id:int}/toggle")]
    [ProducesResponseType(typeof(TaskResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Toggle(int id)
    {
        var task = await taskService.ToggleCompleteAsync(id);
        return task is null ? NotFound(new { message = $"Task {id} not found." }) : Ok(task);
    }
}
