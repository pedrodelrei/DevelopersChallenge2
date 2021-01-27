using System;
using System.Collections.Generic;
using LiteDB;
using System.Linq;

namespace Backend.Models
{
    public class TransactionModel
    {
        public static void ImportTransactions(string accountId, List<Transaction> transactions)
        {
            var oldTransactions = ListTransactions(accountId);
            List<Transaction> newTransactions = new List<Transaction>();
            foreach (var transaction in oldTransactions)
            {
                var savedTransactions = transactions.Where(x => x.Date == transaction.Date && x.Value == transaction.Value);
                newTransactions = transactions.Except(savedTransactions).ToList();
            }

            DataContext.transactions.InsertBulk(newTransactions);
            AccountModel.UpdateAccountTransactions(accountId, newTransactions);
        }

        public static IEnumerable<Transaction> ListTransactions(string accountId)
        {
            var db = DataContext.db;
            var account = DataContext.accounts.Include(x=> x.Transactions).FindById(accountId);
            if (account == null)
                throw new Exception("Account not found");
            return account.Transactions.ToList();
        }
    }
}