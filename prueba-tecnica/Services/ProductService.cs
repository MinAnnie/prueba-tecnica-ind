using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba_tecnica.Models;
using prueba_tecnica.Repositories;

namespace prueba_tecnica.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly StockMovementRepository _stockMovementRepository;

        public ProductService(ProductRepository productRepository, StockMovementRepository stockMovementRepository)
        {
            _productRepository = productRepository;
            _stockMovementRepository = stockMovementRepository;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product.Stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }

            await _productRepository.CreateProductAsync(product);

            var stockMovement = new StockMovement
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                Type = "Entrada"
            };

            await _stockMovementRepository.CreateStockMovementAsync(stockMovement);

            // Devuelve el producto creado después de la operación
            return product;
        }


        public async Task UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new ArgumentException("Producto no encontrado.");
            }

            int stockDifference = product.Stock - existingProduct.Stock;

            await _productRepository.UpdateProductAsync(product);

            if (stockDifference != 0)
            {
                var stockMovement = new StockMovement
                {
                    ProductId = product.Id,
                    Quantity = Math.Abs(stockDifference),
                    Type = stockDifference > 0 ? "Entrada" : "Salida",
                    Date = DateTime.UtcNow
                };

                await _stockMovementRepository.CreateStockMovementAsync(stockMovement);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new ArgumentException("Producto no encontrado.");
            }

            var stockMovement = new StockMovement
            {
                ProductId = product.Id,
                Quantity = product.Stock,
                Date = DateTime.UtcNow,
                Type = "Salida"
            };

            await _stockMovementRepository.CreateStockMovementAsync(stockMovement);
            await _productRepository.DeleteProductAsync(product);
        }
    }
}