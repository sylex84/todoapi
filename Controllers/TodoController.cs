﻿using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Dtos;
using TodoApi.Entities;

namespace TodoApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoController(TodoContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TodoItemDto>>> Get()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();
        
        var todos = await context.TodoItems
            .Where(t => t.UserId == Guid.Parse(userId))
            .ToListAsync();
        return Ok(todos.Adapt<List<TodoItemDto>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetById(int id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        
        return Ok(todoItem.Adapt<TodoItemDto>());
    }
    
    [HttpPost]
    public async Task<ActionResult<TodoItemDto>> Create(CreateTodoItemDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var todoItem = createDto.Adapt<TodoItem>();
        todoItem.UserId = Guid.Parse(userId);
        
        context.TodoItems.Add(todoItem);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = todoItem.Id }, todoItem.Adapt<TodoItemDto>());
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateTodoItemDto updateDto)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();
        if (Guid.Parse(userId) != todoItem.UserId) return Forbid();
        
        updateDto.Adapt(todoItem);
        await context.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var todoItem = await context.TodoItems.FindAsync(id);
        if (todoItem == null) return NotFound();
        
        context.TodoItems.Remove(todoItem);
        await context.SaveChangesAsync();
        return NoContent();
    }
}