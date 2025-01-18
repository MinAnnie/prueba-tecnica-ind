using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prueba_tecnica.Models;

namespace prueba_tecnica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductController(ProductContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound($"El producto con ID {id} no fue encontrado.");
            }

            return product;
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("El ID del producto no coincide con el ID proporcionado.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound($"El producto con ID {id} no fue encontrado.");
            }

            int stockDifference = product.Stock - existingProduct.Stock;

            _context.Entry(existingProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

               
                if (stockDifference != 0)
                {
                    var stockMovement = new StockMovement
                    {
                        ProductId = product.Id,
                        Quantity = Math.Abs(stockDifference),
                        Date = DateTime.UtcNow,
                        Type = stockDifference > 0 ? "Entrada" : "Salida"
                    };

                    _context.StockMovements.Add(stockMovement);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound($"El producto con ID {id} no fue encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (product.Stock < 0)
            {
                return BadRequest("El stock no puede ser negativo.");
            }
            
            var stockMovement = new StockMovement
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                Date = DateTime.UtcNow,
                Type = "Entrada"
            };

            _context.StockMovements.Add(stockMovement);
            await _context.SaveChangesAsync();

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound($"El producto con ID {id} no fue encontrado.");
            }
            
            var stockMovement = new StockMovement
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                Date = DateTime.UtcNow,
                Type = "Salida"
            };

            _context.StockMovements.Add(stockMovement);

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
