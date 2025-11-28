using EFCore.GenericRepository.Sample.Entities;
using EFCore.GenericRepository.Sample.Data;
using EFCore.GenericRepository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EFCore.GenericRepository.Extensions;

// --- Setup the Dependency Injection Container ---
// Using the .NET Generic Host for dependency injection, logging, and configuration
var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    // 1. Add the DbContext to the services collection.
    // We are using the InMemory provider for this sample.
    // In a real application, you would use a real database provider like SQL Server, PostgreSQL, etc.
    services.AddDbContext<SampleDbContext>(options =>
        options.UseInMemoryDatabase("GenericRepositorySampleDb"));

    // 2. Add the Generic Repository to the services collection.
    // This extension method registers the IUnitOfWork and the generic repositories.
    // It automatically discovers the DbContext registered in the previous step.
    services.AddGenericRepository<SampleDbContext>();
});

var host = builder.Build();

Console.WriteLine("--- EFCore.GenericRepository Sample Application ---");
Console.WriteLine("Demonstrates the key features of the library.");
Console.WriteLine();

// All database operations will be performed within this scope
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var unitOfWork = services.GetRequiredService<IUnitOfWork<SampleDbContext>>();

    await RunSampleAsync(unitOfWork);
}

Console.WriteLine();
Console.WriteLine("--- Sample Application Finished ---");


/// <summary>
/// Main execution method for the sample.
/// </summary>
static async Task RunSampleAsync(IUnitOfWork<SampleDbContext> unitOfWork)
{
    // --- 1. Seeding the Database ---
    // Let's add some initial data to our in-memory database.
    Console.WriteLine("--- 1. Seeding Database ---");
    await SeedDatabaseAsync(unitOfWork);
    Console.WriteLine("Database seeded with 2 categories and 5 products.");
    Console.WriteLine();


    // --- 2. Basic CRUD Operations ---
    Console.WriteLine("--- 2. Basic CRUD Operations ---");

    // Get the repository for the Product entity
    // The key type for Product is <int>
    var productRepository = unitOfWork.Repository<Product, int>();

    // **CREATE**: Add a new product
    Console.WriteLine("Adding a new product: 'Gaming Mouse'");
    var newProduct = new Product { Name = "Gaming Mouse", Price = 79.99m, CategoryId = 1 };
    await productRepository.AddAsync(newProduct);
    await unitOfWork.SaveChangesAsync();
    Console.WriteLine($"New product added with ID: {newProduct.Id}");
    Console.WriteLine();

    // **READ**: Get a product by its ID
    Console.WriteLine($"Fetching product with ID: {newProduct.Id}");
    var fetchedProduct = await productRepository.GetByIdAsync(newProduct.Id);
    Console.WriteLine($"Fetched: {fetchedProduct?.Name} - Price: {fetchedProduct?.Price:C}");
    Console.WriteLine();

    // **UPDATE**: Change the price of the product
    Console.WriteLine($"Updating price for product ID: {newProduct.Id}");
    fetchedProduct!.Price = 74.99m;
    productRepository.Update(fetchedProduct);
    await unitOfWork.SaveChangesAsync();
    
    var updatedProduct = await productRepository.GetByIdAsync(newProduct.Id);
    Console.WriteLine($"Updated Price: {updatedProduct?.Price:C}");
    Console.WriteLine();

    // **DELETE (Soft Delete)**: Delete a product
    // Since Product implements ISoftDeletable, this will be a soft delete.
    Console.WriteLine($"Soft deleting product ID: {newProduct.Id}");
    await productRepository.DeleteAsync(newProduct.Id);
    await unitOfWork.SaveChangesAsync();

    // Verify it's gone (should not be found)
    var deletedProduct = await productRepository.GetByIdAsync(newProduct.Id);
    Console.WriteLine($"Product found after delete: {(deletedProduct == null ? "No" : "Yes")}");
    Console.WriteLine();


    // --- 3. Querying, Sorting, and Pagination ---
    Console.WriteLine("--- 3. Querying, Sorting, and Pagination ---");
    
    // **FIND**: Find all products with a price greater than $500
    Console.WriteLine("Finding all products with price > $500...");
    var expensiveProducts = await productRepository
        .FindAsync(p => p.Price > 500);

    foreach (var p in expensiveProducts)
    {
        Console.WriteLine($"- {p.Name} ({p.Price:C})");
    }
    Console.WriteLine();

    // **INCLUDE**: Eager load related entities (Categories)
    Console.WriteLine("Getting all products including their categories...");
    var productsWithCategories = await productRepository
        .Include(p => p.Category)
        .GetAllAsync();
        
    foreach (var p in productsWithCategories)
    {
        Console.WriteLine($"- {p.Name} (Category: {p.Category.Name})");
    }
    Console.WriteLine();
    
    // **SORTING**: Get all products sorted by price descending
    Console.WriteLine("Getting all products sorted by price (descending)...");
    var sortedProducts = await productRepository
        .OrderByDescending(p => p.Price)
        .GetAllAsync();

    foreach (var p in sortedProducts)
    {
        Console.WriteLine($"- {p.Name} ({p.Price:C})");
    }
    Console.WriteLine();

    // **PAGINATION**: Get the second page of products (2 items per page)
    Console.WriteLine("Getting page 2 of products (2 per page)...");
    var pagedResult = await productRepository
        .OrderBy(p => p.Id) // It's important to have a consistent order for pagination
        .GetPagedAsync(pageNumber: 2, pageSize: 2);

    Console.WriteLine($"Total Products: {pagedResult.TotalCount}, Total Pages: {pagedResult.TotalPages}");
    foreach (var p in pagedResult.Items)
    {
        Console.WriteLine($"- Page 2, Item: {p.Name}");
    }
    Console.WriteLine();


    // --- 4. Soft Delete and Restore ---
    Console.WriteLine("--- 4. Soft Delete and Restore ---");
    
    // We soft-deleted product ID 6 earlier. Let's confirm it's not in the main list.
    var allProducts = await productRepository.GetAllAsync();
    Console.WriteLine($"Total products found (excluding soft-deleted): {allProducts.Count()}");
    
    // **RESTORE**: Now, let's restore the deleted product.
    Console.WriteLine($"Restoring product with ID: {newProduct.Id}");
    await productRepository.RestoreAsync(newProduct.Id);
    await unitOfWork.SaveChangesAsync();

    // Verify it has been restored
    var restoredProduct = await productRepository.GetByIdAsync(newProduct.Id);
    Console.WriteLine($"Product found after restore: {(restoredProduct != null ? "Yes" : "No")}");
    Console.WriteLine($"Total products are now: {(await productRepository.GetAllAsync()).Count()}");
    Console.WriteLine();
}

/// <summary>
/// Seeds the database with initial data.
/// </summary>
static async Task SeedDatabaseAsync(IUnitOfWork<SampleDbContext> unitOfWork)
{
    // Using repositories from the UnitOfWork to add data
    var categoryRepository = unitOfWork.Repository<Category, int>();
    var productRepository = unitOfWork.Repository<Product, int>();

    // Check if data already exists
    if (await categoryRepository.CountAsync() > 0)
    {
        Console.WriteLine("Database already seeded. Skipping.");
        return;
    }

    // Create Categories
    var electronics = new Category { Name = "Electronics" };
    var books = new Category { Name = "Books" };
    await categoryRepository.AddRangeAsync(new[] { electronics, books });

    // Create Products
    var products = new List<Product>
    {
        new() { Name = "Laptop Pro", Price = 1499.99m, Category = electronics },
        new() { Name = "Wireless Keyboard", Price = 89.99m, Category = electronics },
        new() { Name = "4K Monitor", Price = 799.00m, Category = electronics },
        new() { Name = "The Pragmatic Programmer", Price = 45.50m, Category = books },
        new() { Name = "Clean Architecture", Price = 39.99m, Category = books }
    };
    await productRepository.AddRangeAsync(products);

    // Save all changes to the database in a single transaction
    await unitOfWork.SaveChangesAsync();
}