using System.ComponentModel.DataAnnotations;

namespace DeliveryStorage.API.Attributes;

public class UniqueItemsAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is IEnumerable<Guid> items)
        {
            var distinctItems = items.Distinct().ToList();
            
            if (distinctItems.Count != items.Count())
            {
                return new ValidationResult("Все элементы списка должны быть уникальными");
            }
        }
        
        return ValidationResult.Success;
    }
}