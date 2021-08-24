using FluentValidation;
using System;

namespace WebApi
{
    public class UserPostValidator : AbstractValidator<UserPostView>
    {
        public UserPostValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0);
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.SurName).NotEmpty();
            RuleFor(command => command.Birthday.Date).NotEmpty().LessThan(DateTime.Today.Date);
            RuleFor(command => command.address.State).NotEmpty();
            RuleFor(command => command.address.State).NotEmpty();

        }
    }
}