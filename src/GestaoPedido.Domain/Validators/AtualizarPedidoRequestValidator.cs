using FluentValidation;
using GestaoPedido.Domain.Interfaces.UseCases.PedidoUseCases.Requests;

namespace GestaoPedido.Domain.Validators;
public class AtualizarPedidoRequestValidator : AbstractValidator<AtualizarPedidoRequest>
{
    public AtualizarPedidoRequestValidator()
    {
        RuleFor(x => x.ClienteId)
            .NotEmpty().WithMessage("Cliente é obrigatório.");

        RuleFor(x => x.Data)
            .NotEmpty().WithMessage("Data é obrigatória.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido.");

        RuleFor(x => x.Itens)
            .NotEmpty().WithMessage("É necessário adicionar ao menos um item ao pedido.");

        RuleForEach(x => x.Itens)
            .SetValidator(new ItemPedidoRequestValidator());
    }
}
