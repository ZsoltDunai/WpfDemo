namespace WpfDemo.App.Models;

public sealed class Product
{
    public required string Name { get; set; }

    public decimal Price { get; set; }

    public bool IsFeatured { get; set; }

    public string DisplayText => $"{(IsFeatured ? "[Featured] " : string.Empty)}{Name} - ${Price:0.00}";

    public Product Clone()
    {
        return new Product
        {
            Name = Name,
            Price = Price,
            IsFeatured = IsFeatured,
        };
    }
}
