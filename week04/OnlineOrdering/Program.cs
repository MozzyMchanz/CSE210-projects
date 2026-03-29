using System;
using System.Collections.Generic;
using System.Text;

public class Address
{
    private string street;
    private string city;
    private string state;
    private string country;

    public Address(string street, string city, string state, string country)
    {
        this.street = street;
        this.city = city;
        this.state = state;
        this.country = country;
    }

    public bool IsInUSA()
    {
        return country == "USA";
    }

    public override string ToString()
    {
        return $"{street}\n{city}, {state}\n{country}";
    }
}

public class Customer
{
    private string name;
    private Address address;

    public Customer(string name, Address address)
    {
        this.name = name;
        this.address = address;
    }

    public string Name => name;
    public Address Address => address;

    public bool IsInUSA()
    {
        return address.IsInUSA();
    }
}

public class Product
{
    private string name;
    private string productId;
    private double price;
    private int quantity;

    public Product(string name, string productId, double price, int quantity)
    {
        this.name = name;
        this.productId = productId;
        this.price = price;
        this.quantity = quantity;
    }

    public string Name => name;
    public string ProductId => productId;

    public double GetTotalCost()
    {
        return price * quantity;
    }
}

public class Order
{
    private List<Product> products;
    private Customer customer;

    public Order(Customer customer)
    {
        this.customer = customer;
        products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        products.Add(product);
    }

    public double TotalPrice()
    {
        double subtotal = 0;
        foreach (Product p in products)
        {
            subtotal += p.GetTotalCost();
        }
        double shipping = customer.IsInUSA() ? 5 : 35;
        return subtotal + shipping;
    }

    public string PackingLabel()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Product p in products)
        {
            sb.AppendLine($"{p.Name} ({p.ProductId})");
        }
        return sb.ToString();
    }

    public string ShippingLabel()
    {
        return $"{customer.Name}\n{customer.Address}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        // USA Customer
        Address usaAddress = new Address("123 Main St", "Provo", "UT", "USA");
        Customer usaCustomer = new Customer("John Doe", usaAddress);

        Order order1 = new Order(usaCustomer);
        order1.AddProduct(new Product("Widget A", "001", 2.50, 3));
        order1.AddProduct(new Product("Gadget B", "002", 5.00, 2));
        order1.AddProduct(new Product("Tool C", "003", 1.75, 4));

        Console.WriteLine("Order 1:");
        Console.WriteLine("Packing Label:");
        Console.WriteLine(order1.PackingLabel());
        Console.WriteLine("Shipping Label:");
        Console.WriteLine(order1.ShippingLabel());
        Console.WriteLine($"Total: ${order1.TotalPrice():F2}");
        Console.WriteLine();

        // International Customer
        Address canAddress = new Address("456 Maple Ave", "Toronto", "ON", "Canada");
        Customer canCustomer = new Customer("Jane Smith", canAddress);

        Order order2 = new Order(canCustomer);
        order2.AddProduct(new Product("Super Widget", "004", 10.00, 1));
        order2.AddProduct(new Product("Mega Gadget", "005", 15.50, 2));

        Console.WriteLine("Order 2:");
        Console.WriteLine("Packing Label:");
        Console.WriteLine(order2.PackingLabel());
        Console.WriteLine("Shipping Label:");
        Console.WriteLine(order2.ShippingLabel());
        Console.WriteLine($"Total: ${order2.TotalPrice():F2}");
    }
}

