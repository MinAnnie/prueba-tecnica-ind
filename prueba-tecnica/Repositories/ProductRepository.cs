using Microsoft.EntityFrameworkCore;
using prueba_tecnica.Models;
using prueba_tecnica.Repositories;
using prueba_tecnica.Services;
using prueba_tecnica.Utils.Errors;

namespace prueba_tecnica.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await _context.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new AppException(ErrorCodes.ProductErrors.ProductCreationFailed,
                "Error al obtener todos los productos.", ex);
        }
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new AppException(ErrorCodes.ProductErrors.ProductNotFound,
                    $"Producto con ID {id} no encontrado.");
            }

            return product;
        }
        catch (Exception ex)
        {
            throw new AppException(ErrorCodes.ProductErrors.ProductNotFound,
                $"Error al obtener el producto con ID {id}.", ex);
        }
    }

    public async Task CreateProductAsync(Product product)
    {
        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new AppException(ErrorCodes.ProductErrors.ProductCreationFailed, "Error al crear el producto.", ex);
        }
    }

    public async Task UpdateProductAsync(Product product)
    {
        try
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new AppException(ErrorCodes.ProductErrors.ProductUpdateFailed, "Error al actualizar el producto.",
                ex);
        }
    }

    public async Task DeleteProductAsync(int id)
    {
        try
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new AppException(ErrorCodes.ProductErrors.ProductNotFound,
                    $"Producto con ID {id} no encontrado.");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new AppException(ErrorCodes.ProductErrors.ProductDeleteFailed, "Error al eliminar el producto.", ex);
        }
    }
}