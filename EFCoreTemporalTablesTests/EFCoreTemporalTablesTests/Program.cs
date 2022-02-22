using EFCoreTemporalTablesTests;
using Microsoft.EntityFrameworkCore;

var id = await CreateProduct();

await UpdateProduct(id);

await RemoveProduct(id);

await DisplayHistoryWithDates(id);

static async Task<Guid> CreateProduct()
{
    await using DataContext db = new DataContext();

    var product = new Product()
    {
        Id = Guid.NewGuid(),
        Name = "product name",
        Description = "product description"
    };

    await db.Products.AddAsync(product);

    await db.SaveChangesAsync();

    return product.Id;
}

static async Task UpdateProduct(Guid id)
{
    await using DataContext db = new DataContext();

    var product = await db.Products.FirstAsync(p => p.Id == id);

    product.Name = "new product name";

    await db.SaveChangesAsync();
}

static async Task RemoveProduct(Guid id)
{
    await using DataContext db = new DataContext();

    db.Entry(new Product() {Id = id}).State = EntityState.Deleted;

    await db.SaveChangesAsync();
}

static async Task DisplayHistoryWithDates(Guid id)
{
    await using DataContext db = new DataContext();

    var historyItems = await db.Products
        .TemporalAll()
        .Where(p => p.Id == id)
        .Select(p => new
        {
            p.Name,
            p.Description,
            PeriodStart = EF.Property<DateTime>(p, "PeriodStart"),
            PeriodEnd = EF.Property<DateTime>(p, "PeriodEnd")
        })
        .ToListAsync();

    foreach (var item in historyItems)
    {
        Console.WriteLine($"{item.Name}: {item.Description}, Start: {item.PeriodStart}, End: {item.PeriodEnd}");
    }
}