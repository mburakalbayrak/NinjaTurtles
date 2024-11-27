using FluentValidation;
using NinjaTurtles.Business.Constants;
using NinjaTurtles.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Business.ValidationRules.FluentValidation
{
    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.Email).NotEmpty().WithMessage(ValidationMessages.CustomerEmailNotEmpty);
            RuleFor(c => c.Email).EmailAddress();
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.PhoneNumber).Matches(@"^\+90\d{10}$").WithMessage("Telefon numarası geçerli bir formatta olmalıdır. (+90XXXXXXXXXX)");

        }
    }
}
