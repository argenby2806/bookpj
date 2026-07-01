using FluentValidation;
using bookpj.Entities;
using System;

namespace bookpj.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Tiêu đề sách không được để trống.")
                .MaximumLength(250).WithMessage("Tiêu đề sách không được vượt quá 250 ký tự.");

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage("Tên tác giả không được để trống.")
                .MaximumLength(250).WithMessage("Tên tác giả không được vượt quá 25- ký tự.");

            RuleFor(book => book.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá sách không được là số âm.");

            When(book => !book.IsAvailable, () =>
            {
                RuleFor(book => book.BorrowedAT)
                    .NotEmpty().WithMessage("Ngày mượn không được để trống khi sách đã được mượn.")
                    .LessThanOrEqualTo(DateTime.Now).WithMessage("Ngày mượn không thể lớn hơn thời gian hiện tại.");

                RuleFor(book => book.DueDate)
                    .NotEmpty().WithMessage("Hạn trả sách không được để trống.")
                    .GreaterThan(book => book.BorrowedAT).WithMessage("Hạn trả sách phải sau ngày mượn sách.");
            });

        }
    }
}