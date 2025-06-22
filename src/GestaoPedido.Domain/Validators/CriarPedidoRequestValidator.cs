using FluentValidation;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

namespace GestaoPedido.Domain.Validators;
public class CriarPedidoRequestValidator : AbstractValidator<CriarPedidoRequest>
{
    public CriarPedidoRequestValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty().WithMessage("Cliente é obrigatório.");

        RuleFor(x => x.Itens)
            .NotEmpty().WithMessage("É necessário adicionar pelo menos um produto.");

        RuleForEach(x => x.Itens)
            .SetValidator(new ItemPedidoRequestValidator());
    }
}

