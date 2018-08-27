using System;

public class Transaction : IComparable<Transaction>
{
    public int Id { get; set; }
    public TransactionStatus Status { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public double Amount { get; set; }

    public Transaction(int id, TransactionStatus st, string from, string to, double amount)
    {
        this.Id = id;
        this.Status = st;
        this.From = from;
        this.To = to;
        this.Amount = amount;
    }

    public int CompareTo(Transaction other)
    {
        var cmp = this.Amount.CompareTo(other.Amount);
        if (cmp == 0)
        {
            cmp = other.Id.CompareTo(this.Id);
        }

        return cmp;
    }
}