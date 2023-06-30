using Microsoft.EntityFrameworkCore;
using MinimalAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseInMemoryDatabase("Employees"));
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


/*****************************API CHECKING***************************************************/
app.MapGet("/", () => "Hello World!");

/*****************************GET - EMPLOYEES LIST*******************************************/
app.MapGet("/EmployeesList", async (ApplicationDbContext db) =>
    await db.Employee.ToListAsync());

/*****************************GET - EMPLOYEE BY ID*******************************************/
app.MapGet("/GetEmployeeByID/{id}", async (Guid id, ApplicationDbContext db) =>
    await db.Employee.FindAsync(id)
        is Employee todo
        ? Results.Ok(todo)
        : Results.NotFound());

/*****************************POST - ADD EMPLOYEE TO LIST************************************/
app.MapPost("/AddEmployeeDetails", async (Employee employee, ApplicationDbContext db) => {
    db.Employee.Add(employee);
    await db.SaveChangesAsync();
    return Results.Created($"/AddEmployeeDetails/{employee.Id}", employee);
});

/*****************************PUT - UPDATE EMPLOYEE DETAILS**********************************/
app.MapPut("/UpdateEmployeeDetails/{id}", async (Guid id, Employee inputTodo, ApplicationDbContext db) =>
{
    var todo = await db.Employee.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Name = inputTodo.Name;
    todo.Age = inputTodo.Age;
    todo.Salary = inputTodo.Salary;
    todo.Experience = inputTodo.Experience;
    todo.Role = inputTodo.Role;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

/*****************************DELETE - DELETE AN EMPLOYEE DETAIL****************************/
app.MapDelete("/DeleteAnEmployeeDetail/{id}", async (Guid id, ApplicationDbContext db) =>
{
    if (await db.Employee.FindAsync(id) is Employee todo)
    {
        db.Employee.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }
    return Results.NotFound();
});

app.Run();
