using FluentValidation;
using bookpj.Entities;

namespace bookpj.Validator
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(order => order.UserName)
                .NotEmpty().WithMessage("Tên người dùng không được để trống.")
                .MinimumLength(3).WithMessage("Tên người dùng phải có ít nhất 3 ký tự.")
                .MaximumLength(100).WithMessage("Tên người dùng không được vượt quá 100 ký tự.");
            
            RuleForEach(order => order.DetailOrders)
                .SetValidator(new OrderDetailValidator());
        }
    }
}
