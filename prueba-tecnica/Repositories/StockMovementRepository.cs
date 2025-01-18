using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using prueba_tecnica.Models;

namespace prueba_tecnica.Repositories
{
    public class StockMovementRepository
    {
        private readonly ProductContext _context;

        public StockMovementRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task CreateStockMovementAsync(StockMovement stockMovement)
        {
            _context.StockMovements.Add(stockMovement);
            await _context.SaveChangesAsync();
        }
    }
}