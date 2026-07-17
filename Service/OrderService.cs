using bookpj.DTO;
using bookpj.Entities;
using bookpj.Extension;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace bookpj.Service
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _context;
        private readonly ILogger<OrderService> _logger;
        private readonly IValidator<Order> _orderValidator;

        public OrderService(DataContext context, ILogger<OrderService> logger, IValidator<Order> orderValidator)
        {
            _context = context;
            _logger = logger;
            _orderValidator = orderValidator;
        }

        public async Task<List<OrderDTO>> GetAllAsync()
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.DetailOrders)
                    .Select(o => new OrderDTO
                    {
                        Id = o.id,
                        UserName = o.UserName,
                        OrderDate = o.OrderDate,
                        TrackingCode = o.TrackingCode,
                        DetailOrders = o.DetailOrders.Select(d => new DetailOrderDTO
                        {
                            id = d.id,
                            Title = d.Title,
                            Author = d.Author,
                            Price = d.Price
                        }).ToList()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy danh sách toàn bộ đơn hàng.");
                throw;
            }
        }

        public async Task<OrderDTO?> GetByIdAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.DetailOrders)
                    .FirstOrDefaultAsync(o => o.id == id);

                if (order == null) return null;

                return new OrderDTO
                {
                    Id = order.id,
                    UserName = order.UserName,
                    OrderDate = order.OrderDate,
                    DetailOrders = order.DetailOrders.Select(d => new DetailOrderDTO
                    {
                        id = d.id,
                        Title = d.Title,
                        Author = d.Author,
                        Price = d.Price
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin đơn hàng với Id: ", id);
                throw;
            }
        }

        public async Task<bool> CreateAsync(CreateOrderDTO dto)
        {
            try
            {
                var trackingCode = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6)}";

                var newOrder = new Order
                {
                    UserName = dto.UserName,
                    OrderDate = DateTime.Now,
                    TrackingCode = trackingCode,
                    DetailOrders = dto.DetailOrders.Select(d => new DetailOrder
                    {
                        Title = d.Title,
                        Author = d.Author,
                        Price = d.Price,
                        OrderTrackingCode = trackingCode
                    }).ToList()
                };
                

                var validation = await _orderValidator.ValidateAsync(newOrder);
                if (!validation.IsValid)
                {
                    _logger.LogWarning("Validation failed when creating order for user {UserName}: {Errors}",
                        dto.UserName,
                        string.Join("; ", validation.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
                    throw new ValidationException(validation.Errors);
                }
                await _context.Orders.AddAsync(newOrder);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo đơn hàng mới cho User: {UserName}", dto.UserName);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDTO dto)
        {
            try
            {
                var existingOrder = await _context.Orders
                    .Include(o => o.DetailOrders)
                    .FirstOrDefaultAsync(o => o.id == id);

                if (existingOrder == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng với Id: ", id);
                    return false;
                }

                existingOrder.UserName = dto.UserName;
                var trackingCode = existingOrder.TrackingCode ?? $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N").Substring(0, 6)}";


                existingOrder.DetailOrders = dto.DetailOrders.Select(d => new DetailOrder
                {
                    Title = d.Title,
                    Author = d.Author,
                    Price = d.Price,
                    OrderTrackingCode = trackingCode
                }).ToList();

                var validation = await _orderValidator.ValidateAsync(existingOrder);
                if (!validation.IsValid)
                {
                    _logger.LogWarning("Validation failed when updating order id : {Errors}",
                        id,
                        string.Join("; ", validation.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")));
                    throw new ValidationException(validation.Errors);
                }

                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật đơn hàng với Id: ", id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.DetailOrders)
                    .FirstOrDefaultAsync(o => o.id == id);

                if (order == null)
                {
                    _logger.LogWarning("Không tìm thấy đơn hàng với Id:  ", id);
                    return false;
                }

                _context.Orders.Remove(order);
                var result = await _context.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi xóa đơn hàng với Id: ", id);
                throw;
            }
        }
    }
}