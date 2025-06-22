namespace GestaoPedido.Domain.Validators
{
    using FluentValidation;
    using GestaoPedido.Domain.Interfaces.UseCases.ClienteUseCases.Requests;

    public class CriarClienteRequestValidator : AbstractValidator<CriarClienteRequest>
    {
        public CriarClienteRequestValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200);

            RuleFor(x => x.Documento)
                .NotEmpty().WithMessage("O documento (CPF ou CNPJ) é obrigatório.")
                .MaximumLength(20);
        }
    }

}
