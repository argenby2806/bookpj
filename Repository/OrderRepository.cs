using bookpj.Entities;
using bookpj.Extension;
using Microsoft.EntityFrameworkCore;

namespace bookpj.Repository
{
    public class OrderRepository : IOrderRepository
    
        {
            private readonly DataContext _context;
            public OrderRepository(DataContext context)
            {
                _context = context;
            }
            public async Task<List<Order>> GetAllAsync()
            {
                return await _context.Orders.Include(order => order.UserName).
                Include(order => order.DetailOrders).ToListAsync();
            }
            public async Task<Order?> GetByIdAsync(int id)
            {
                return await _context.Orders.FindAsync(id);
            }
            public async Task<Order?> AddAsync(Order order)
            {
                _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return order;
            }
            public async Task UpdateAsync(Order order)
            {
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            }
            public async Task DeleteAsync(Order order)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }
    }

