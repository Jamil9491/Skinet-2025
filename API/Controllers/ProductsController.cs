using System;
using Core.Entities;
using Core.Interfaces;
using Infrastracture.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repo) : ControllerBase
{

   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repo.GetProductsAsync(brand,type, sort));

    }
    [HttpGet("{id:int}")]

    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
         repo.AddProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
         }
        return BadRequest("Problem in creating product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !productExist(id)) return BadRequest("Cannot update this product");

        repo.UpdateProduct(product);
        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating the product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null) return NotFound();
        repo.DeleteProduct(product);
         if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem Deleting the product");
    }

    [HttpGet("Brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repo.GetBrandsAsync());
    }
    [HttpGet("Types")]
     public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repo.GetTypesAsync());
    }
    public bool productExist(int id)
    {
        return repo.ProductExists(id);
    }

}
