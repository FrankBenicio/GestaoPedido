
using FluentValidation;
using GestaoPedido.Domain.Interfaces.UseCases.ProdutoUseCases.Requests;

namespace GestaoPedido.Domain.Validators;
public class CriarProdutoRequestValidator : AbstractValidator<CriarProdutoRequest>
{
    public CriarProdutoRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(200);

        RuleFor(x => x.Preco)
            .GreaterThan(0).WithMessage("O preço deve ser maior que zero.");
    }
}
