using FluentValidation;

namespace DS.Users.Application.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(50).WithMessage("Exceeded Maximum Length");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("FirstName is required")
                .MaximumLength(50).WithMessage("Exceeded Maximum Length");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("FirstName is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(255).WithMessage("Exceeded Maximum Length");
        }
    }
}
