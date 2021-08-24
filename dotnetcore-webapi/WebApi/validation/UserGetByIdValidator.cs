using FluentValidation;
using System;

namespace WebApi
{
    public class UserGetByIdValidator : AbstractValidator<GetByIdValidator>
    {
        public UserGetByIdValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0);

        }
    }
    public class GetByIdValidator
    {
        public int UserId { get; set; }
    }
}