using In_Memory_Db_Test.Context;
using In_Memory_Db_Test.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace In_Memory_Db_Test.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context )
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductaAsync()
        {
            return await _context.Products.ToListAsync();
        } 

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            return await _context.Products.FirstOrDefaultAsync(d => d.Id == Id);
        }


        public async Task AddProductAsync(Product p)
        {
            await _context.Products.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteProductByIdAsync(int Id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == Id);
            
            if(product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
