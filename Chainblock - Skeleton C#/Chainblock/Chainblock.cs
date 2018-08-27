using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

public class Chainblock : IChainblock
{
    private Dictionary<int, Transaction> byId;

    public Chainblock()
    {
        this.byId = new Dictionary<int, Transaction>();
    }

    public int Count
    {
        get
        {
            return this.byId.Count;
        }
    }

    public void Add(Transaction tx)
    {
        this.byId.Add(tx.Id, tx);
    }

    public void ChangeTransactionStatus(int id, TransactionStatus newStatus)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new ArgumentException();
        }

        this.byId[id].Status = newStatus;
    }

    public bool Contains(Transaction tx)
    {
        return this.byId.ContainsKey(tx.Id);
    }

    public bool Contains(int id)
    {
        return this.byId.ContainsKey(id);
    }

    public IEnumerable<Transaction> GetAllInAmountRange(double lo, double hi)
    {
        var transactions = this.byId.Values.Where(tr => tr.Amount >= lo && tr.Amount <= hi);

        return transactions;
    }

    public IEnumerable<Transaction> GetAllOrderedByAmountDescendingThenById()
    {
        var transactions = this.byId.Values.OrderByDescending(tr => tr.Amount).ThenBy(tr => tr.Id);

        return transactions;
    }

    public IEnumerable<string> GetAllReceiversWithTransactionStatus(TransactionStatus status)
    {
        var receivers = this.GetByTransactionStatus(status).Select(tr => tr.To);

        return receivers;
    }

    public IEnumerable<string> GetAllSendersWithTransactionStatus(TransactionStatus status)
    {
        var senders = this.GetTransactionsByStatus(status).OrderByDescending(tr => tr.Amount).Select(tr => tr.From);

        return senders;
    }

    public Transaction GetById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        var transaction = this.byId[id];

        return transaction;
    }

    public IEnumerable<Transaction> GetByReceiverAndAmountRange(string receiver, double lo, double hi)
    {
        var transactions = this.GetTransactionsByReceiver(receiver);

        return transactions.Where(tr => tr.Amount >= lo && tr.Amount < hi).OrderByDescending(tr => tr.Amount).ThenBy(tr => tr.Id);
    }

    public IEnumerable<Transaction> GetByReceiverOrderedByAmountThenById(string receiver)
    {
        var transactions = this.GetTransactionsByReceiver(receiver);

        return transactions.OrderByDescending(tr => tr.Amount).ThenBy(tr => tr.Id);
    }

    public IEnumerable<Transaction> GetBySenderAndMinimumAmountDescending(string sender, double amount)
    {
        var transactions = this.GetTransactionsBySender(sender);

        return transactions.Where(tr => tr.Amount > amount).OrderByDescending(tr => tr.Amount);
    }

    public IEnumerable<Transaction> GetBySenderOrderedByAmountDescending(string sender)
    {
        var transactions = this.GetTransactionsBySender(sender);

        return transactions.OrderByDescending(tr => tr.Amount);
    }

    public IEnumerable<Transaction> GetByTransactionStatus(TransactionStatus status)
    {
        var transactions = this.GetTransactionsByStatus(status).OrderByDescending(tr => tr.Amount);

        return transactions;
    }

    public IEnumerable<Transaction> GetByTransactionStatusAndMaximumAmount(TransactionStatus status, double amount)
    {
        var transactions = this.byId.Values.Where(tr => tr.Status == status);
        if (!transactions.Any())
        {
            return new List<Transaction>();
        }

        return transactions
                .Where(tr => tr.Amount <= amount)
                .OrderByDescending(tr => tr.Amount);
    }

    public void RemoveTransactionById(int id)
    {
        if (!this.byId.ContainsKey(id))
        {
            throw new InvalidOperationException();
        }

        this.byId.Remove(id);
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        return this.byId.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    private IEnumerable<Transaction> GetTransactionsByStatus(TransactionStatus status)
    {
        var transactions = this.byId.Values.Where(tr => tr.Status == status);
        if (!transactions.Any())
        {
            throw new InvalidOperationException();
        }

        return transactions;
    }

    private IEnumerable<Transaction> GetTransactionsBySender(string sender)
    {
        var transactions = this.byId.Values.Where(tr => tr.From == sender);
        if (!transactions.Any())
        {
            throw new InvalidOperationException();
        }

        return transactions;
    }

    private IEnumerable<Transaction> GetTransactionsByReceiver(string receiver)
    {
        var transactions = this.byId.Values.Where(tr => tr.To == receiver);
        if (!transactions.Any())
        {
            throw new InvalidOperationException();
        }

        return transactions;
    }
}

