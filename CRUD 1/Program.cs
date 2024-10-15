var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
/*
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
*/

// CRUD de Empleados:
var empleados = new List<Empleado> {
    new Empleado {id = 1, name = "Jose Luis", puesto = "Frontend", salario = "28000USD", companiaId = 1 },
    new Empleado {id = 2, name = "Pedrito", puesto = "Backend", salario = "30000USD" ,companiaId = 2 }
};

app.MapGet("/empleados", () =>
{
    return Results.Ok(empleados);
});
app.MapGet("/empleado/{id}", (int id) =>
{
    var empleado = empleados.FirstOrDefault(e => e.id == id);
    return empleado != null ? Results.Ok(empleado) : Results.NotFound();
});
app.MapPost("/empleados", (Empleado newEmpleados) =>
{
    newEmpleados.id = empleados.Max(e => e.id) + 1;
    empleados.Add(newEmpleados);
    return Results.Created($"/empleados/{newEmpleados.id}", newEmpleados);
});
app.MapPut("/empleados/{id}", (int id, Empleado updateEmpleado) =>
{
    var empleado = empleados.FirstOrDefault(e => e.id == id);
    if (empleado == null)
    {
        return Results.NotFound();
    }

    empleado.name = updateEmpleado.name;
    empleado.salario = updateEmpleado.salario;

    return Results.NoContent();
});
app.MapDelete("/empleados/{id}", (int id) =>
{
    var empleado = empleados.FirstOrDefault(p => p.id == id);
    if (empleado == null)
    {
        return Results.NotFound();
    }
    empleados.Remove(empleado);
    return Results.NoContent();
});
// CRUD de compania
var companias = new List<Compania>{
    new Compania {id = 1, name = "Claro"},
    new Compania {id = 2, name = "Altice"}
};
app.MapGet("/companias", () =>
{
    return Results.Ok(companias);
});
app.MapGet("/compania/{id}", (int id) =>
{
    var compania = companias.FirstOrDefault(c => c.id == id);
    return compania != null ? Results.Ok(compania) : Results.NotFound();
});
app.MapPost("/companias", (Compania newCompania) =>
{
    newCompania.id = companias.Max(c => c.id) + 1;
    companias.Add(newCompania);
    return Results.Created($"/empleados/{newCompania.id}", newCompania);
});
app.MapPut("/Companias/{id}", (int id, Compania updateCompania) =>
{
    var compania = companias.FirstOrDefault(c => c.id == id);
    if (compania == null)
    {
        return Results.NotFound();
    }

    compania.name = updateCompania.name;

    return Results.NoContent();
});
app.MapDelete("/companias/{id}", (int id) =>
{
    var compania = companias.FirstOrDefault(c => c.id == id);
    if (compania == null)
    {
        return Results.NotFound();
    }
    companias.Remove(compania);
    return Results.NoContent();
});

// Delete en general, tanto Compañia con empleado
app.MapDelete("/compania/{id}/conEmpleados", (int id) =>
{
    var compania = companias.FirstOrDefault(c => c.id == id);
    if (compania == null)
    {
        return Results.NotFound();
    }
    empleados.RemoveAll(e => e.id == id);
    companias.Remove(compania);

    return Results.NoContent();
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class Empleado
{
    public int id { get; set; }
    public string name { get; set; }
    public string puesto { get; set; }
    public string salario { get; set; }

    public int companiaId { get; set; }
};

public class Compania
{
    public int id { get; set; }
    public string name { get; set; }

};