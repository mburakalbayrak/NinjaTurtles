using FluentValidation;

namespace NinjaTurtles.Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool
    {
        public static void Validate<T>(IValidator<T> validator, T entity)
        {
            var result = validator.Validate(entity);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
