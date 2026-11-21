using System.ComponentModel.DataAnnotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class MaxDecimalPlacesAttribute : ValidationAttribute
{
    private readonly int _places;

    public MaxDecimalPlacesAttribute(int places)
        : base("The field {0} must have at most " + places + " decimal places.")
    {
        _places = places;
    }

    public override bool IsValid(object? value)
    {
        if (value is not decimal number)
        {
            return true;
        }

        return decimal.Round(number, _places) == number;
    }
}
