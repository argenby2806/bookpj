using bookpj.Entities;
using FluentValidation;

namespace bookpj.Validator
{
    public class OrderDetailValidator : AbstractValidator<DetailOrder>
    {
        public OrderDetailValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề sách không được để trống.")
                .MaximumLength(250).WithMessage("Tiêu đề sách không được vượt quá 250 ký tự.");
            RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Tên tác giả không được để trống.")
                .MaximumLength(250).WithMessage("Tên tác giả không được vượt quá 250 ký tự.");
            RuleFor(book => book.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá sách không được là số âm.");
            
        }
    }
}
