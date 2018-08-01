using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Api.Contracts
{
    public class PaymentIntent
    {
        public TransactionAmount Transaction { get; }

        public PaymentIntent(TransactionAmount transaction)
        {
            Transaction = transaction;
        }
    }

    public class PaymentIntentValidator : AbstractValidator<PaymentIntent>
    {
        public PaymentIntentValidator()
        {
            RuleFor(intent => intent.Transaction).NotNull().SetValidator(new TransactionAmountValidator());
        }
    }

    public class TransactionAmount
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public TransactionAmount(decimal amount, string currency)
        {
            Amount = amount;
            Currency = currency;
        }
    }

    public class TransactionAmountValidator : AbstractValidator<TransactionAmount>
    {
        public TransactionAmountValidator()
        {
            RuleFor(transactionAmount => transactionAmount.Amount).GreaterThan(0).WithMessage("InvalidTransactionAmount:The transaction amount must be greater than zero");
            RuleFor(transactionAmount => transactionAmount.Currency).NotEmpty();
        }
    }
}
