using FluentValidation;
using System;

namespace WebApi
{
    public class UserDeleteByIdValidator : AbstractValidator<DeleteByIdValidator>
    {
        public UserDeleteByIdValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0);

        }
    }
    public class DeleteByIdValidator
    {
        public int UserId { get; set; }
    }
}