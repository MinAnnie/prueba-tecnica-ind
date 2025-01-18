using Microsoft.EntityFrameworkCore;
using prueba_tecnica.Models;
using prueba_tecnica.Repositories;
using prueba_tecnica.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<StockMovementRepository>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddDbContext<ProductContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Productos",
        Version = "v1",
        Description = "Una API para gestionar productos y movimientos de inventario.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Angie Matiz",
            Email = "matizangie6@gmail.com"
        }
    });

    var xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "prueba-tecnica.xml");
    c.IncludeXmlComments(xmlFile);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Productos v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();