using bookpj.DTO;

namespace bookpj.Service
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAllAsync();
        Task<OrderDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CreateOrderDTO dto);
        Task<bool> UpdateAsync(int id, UpdateOrderDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
