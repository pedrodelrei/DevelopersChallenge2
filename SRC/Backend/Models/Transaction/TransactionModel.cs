using System;
using System.Collections.Generic;
using LiteDB;
using System.Linq;

namespace Backend.Models
{
    public class TransactionModel
    {
        public static List<Transaction> CreateTransactions(List<Transaction> newTransactions)
        {
            List<Transaction> added = new List<Transaction>();
            var transactions = DataContext.transactions;
            foreach (var transaction in newTransactions)
            {
                if (GetTransaction(transaction) == null)
                {
                    transactions.Insert(transaction);
                    added.Add(transaction);
                }
            }
            return added;
        }

        public static Transaction GetTransaction(Transaction trans)
        {
            return DataContext.transactions.FindOne(x => x.AccountId == trans.AccountId && x.Value == trans.Value && x.Date == trans.Date);
        }

        public static IEnumerable<Transaction> ListTransactions(string accountId)
        {
            var account = DataContext.accounts.Include(x=> x.Transactions).FindById(accountId);
            if (account == null)
                throw new Exception("Account not found");
            return account.Transactions.ToList();
        }

        public static IEnumerable<Transaction> ListTransactions()
        {
            return DataContext.transactions.FindAll();
        }
    }
}