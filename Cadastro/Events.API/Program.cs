var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Lista em memória para armazenar os produtos
List<Product> products = new List<Product>
{
    new Product { Id = 1, Name = "Pão", Description = "Pão fresquinho", Price = 3.99m, Quantity = 150 },
    new Product { Id = 2, Name = "Leite", Description = "Leite orgânico", Price = 2.49m, Quantity = 100 },
    new Product { Id = 3, Name = "Ovos", Description = "Ovos de galinhas criadas soltas", Price = 4.79m, Quantity = 80 }
};

// Rota GET para obter todos os produtos
app.MapGet("/products", () => products);

// Rota GET para obter um produto pelo Id
app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(product);
});

// Rota POST para criar um novo produto
app.MapPost("/products", (Product product) =>
{
    product.Id = products.Count + 1;
    products.Add(product);
    return Results.Created($"/products/{product.Id}", product);
});

// Rota PUT para atualizar um produto existente
app.MapPut("/products/{id}", (int id, Product updatedProduct) =>
{
    var existingProduct = products.FirstOrDefault(p => p.Id == id);
    if (existingProduct == null)
    {
        return Results.NotFound();
    }

    existingProduct.Name = updatedProduct.Name;
    existingProduct.Description = updatedProduct.Description;
    existingProduct.Price = updatedProduct.Price;
    existingProduct.Quantity = updatedProduct.Quantity;

    return Results.Ok(existingProduct);
});

// Rota DELETE para excluir um produto
app.MapDelete("/products/{id}", (int id) =>
{
    var existingProduct = products.FirstOrDefault(p => p.Id == id);
    if (existingProduct == null)
    {
        return Results.NotFound();
    }

    products.Remove(existingProduct);
    return Results.NoContent();
});

app.Run();

// Definição da classe de Produto
public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}