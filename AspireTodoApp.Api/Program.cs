using AspireTodoApp.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// add in memory database
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseInMemoryDatabase("TodoDatabase"));
builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var todosApi = app.MapGroup("/todos");

todosApi.MapGet("/", async () =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var todoItems =  await dbContext.TodoItems.ToListAsync();
    if (todoItems == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(todoItems.Select(todoItem => new GetTodo(todoItem.Id, todoItem.Title, todoItem.IsComplete, todoItem.CreatedAt)));
}).WithOpenApi();

todosApi.MapGet("/{id}", async (int id) =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var todoItem = await dbContext.TodoItems.FindAsync(id);
    if (todoItem == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(new GetTodo(todoItem.Id, todoItem.Title, todoItem.IsComplete, todoItem.CreatedAt));
}).WithOpenApi();

todosApi.MapPost("/", async (string title) =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var todoItem = dbContext.TodoItems.Add(new TodoItem { Title = title });
    await dbContext.SaveChangesAsync();
    return Results.Created();
}).WithOpenApi();

todosApi.MapPut("/{id}", async (int id, UpdateTodoItem todoItem) =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var existingTodoItem = await dbContext.TodoItems.FindAsync(id);
    if (existingTodoItem == null)
    {
        return Results.NotFound();
    }
    existingTodoItem.Title = todoItem.Title;
    existingTodoItem.IsComplete = todoItem.IsComplete;
    await dbContext.SaveChangesAsync();
    return Results.Ok(existingTodoItem);
}).WithOpenApi();

todosApi.MapDelete("/{id}", async (int id) =>
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    var existingTodoItem = await dbContext.TodoItems.FindAsync(id);
    if (existingTodoItem == null)
    {
        return Results.NotFound();
    }
    dbContext.TodoItems.Remove(existingTodoItem);
    await dbContext.SaveChangesAsync();
    return Results.NoContent();
}).WithOpenApi();

app.Run();

public record GetTodo(int Id, string Title, bool IsComplete, DateTime CreatedAt);
public record UpdateTodoItem(string Title, bool IsComplete);

