using FluentValidation;
using GestaoPedido.Domain.Interfaces.UseCases.UserUseCases.Requests;

namespace GestaoPedido.Domain.Validators;

public class CriarUsuarioRequestValidator : AbstractValidator<CriarUsuarioRequest>
{
    public CriarUsuarioRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("O nome de usuário é obrigatório.")
            .MinimumLength(3).WithMessage("O nome de usuário deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve ter pelo menos 6 caracteres.");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("O perfil é obrigatório.");
    }
}
