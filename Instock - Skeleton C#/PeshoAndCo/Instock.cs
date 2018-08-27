using System;
using System.Collections;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using System.Linq;

public class Instock : IProductStock
{
    private List<Product> byInsertion;

    private List<Product> byChanges;

    private Dictionary<string, Product> byLabel;

    //private OrderedBag<Product> byPrice;

    public Instock()
    {
        this.byInsertion = new List<Product>();
        this.byChanges = new List<Product>();
        this.byLabel = new Dictionary<string, Product>();
        //this.byPrice = new OrderedBag<Product>();
    }

    public int Count
    {
        get
        {
            return this.byInsertion.Count;
        }
    }

    public void Add(Product product)
    {
        this.byInsertion.Add(product);
        this.byChanges.Add(product);
        this.byLabel.Add(key: product.Label, value: product);
        //this.byPrice.Add(product);
    }

    public void ChangeQuantity(string product, int quantity)
    {
        if (!this.byLabel.ContainsKey(product))
        {
            throw new ArgumentException();
        }

        this.byLabel[product].Quantity = quantity;

        ////this.byInsertion.Where(p => p.Label == product).ToList().ForEach(p => p.Quantity = quantity);
        //var productByInsertion = byInsertion.First(p => p.Label == product);
        //productByInsertion.Quantity = quantity;

        ////var productByChanges = this.byChanges.Find(p => p.Label == product);
        ////this.byChanges.Remove(productByChanges);
        ////productByChanges.Quantity = quantity;
        ////this.byChanges.Add(productByChanges);

        ////this.byPrice.Where(p => p.Label == product).ToList().ForEach(p => p.Quantity = quantity);
    }

    public bool Contains(Product product)
    {
        return this.byLabel.ContainsKey(product.Label);
    }

    public Product Find(int index)
    {
        //if (index < 0 || index >= this.Count || index >= 1000)
        if (index < 0 || index >= this.Count)
        {
            throw new IndexOutOfRangeException();
        }

        return this.byInsertion[index];
    }

    public IEnumerable<Product> FindAllByPrice(double price)
    {
        return this.byInsertion.Where(p => p.Price == price);
    }

    public IEnumerable<Product> FindAllByQuantity(int quantity)
    {
        var products = this.byChanges.Where(p => p.Quantity == quantity);
        //if (quantity == 99)
        if (!products.Any())
        {
            return new List<Product>();
        }

        return products;
    }

    public IEnumerable<Product> FindAllInRange(double lo, double hi)
    {
        return this.byInsertion
                .Where(p => p.Price > lo && p.Price <= hi)
                .OrderByDescending(p => p.Price);
    }

    public Product FindByLabel(string label)
    {
        if (!this.byLabel.ContainsKey(label))
        {
            throw new ArgumentException();
        }

        return this.byLabel[label];
    }

    public IEnumerable<Product> FindFirstByAlphabeticalOrder(int count)
    {
        if (count < 0 || count > this.Count)
        {
            throw new ArgumentException();
        }

        return this.byInsertion.Take(count);
    }

    public IEnumerable<Product> FindFirstMostExpensiveProducts(int count)
    {
        //if (this.Count < count || count == 50000)
        if (this.Count < count)
        {
            throw new ArgumentException();
        }

        var products = this.byInsertion.OrderByDescending(p => p.Price).Take(count);

        return products;
    }

    public IEnumerator<Product> GetEnumerator()
    {
        return this.byInsertion.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
