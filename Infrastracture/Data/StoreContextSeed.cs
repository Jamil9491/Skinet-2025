using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastracture.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                // Read JSON file asynchronously
                var productData = await File.ReadAllTextAsync("../Infrastracture/Data/SeedData/products.json");

                // Deserialize into list of Product objects
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
      
                if (products != null)
                {
                    // Add products to DB context
                    context.Products.AddRange(products);
                    
                    // Save to database
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
