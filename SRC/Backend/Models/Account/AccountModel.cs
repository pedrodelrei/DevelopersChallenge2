using System;
using System.Collections.Generic;
using LiteDB;
using System.Linq;

namespace Backend.Models
{
    public class AccountModel
    {
        public static Account CreateAccount(Account newAccount)
        {
            // Get user collection
            var accounts = DataContext.accounts;
            if (accounts.FindById(newAccount.Id) != null)
                throw new Exception("Account already exists");
            
            // Insert new user document (Id will be auto-incremented)
            accounts.Insert(newAccount);
            return newAccount;
        }

        public static Account GetAccount(string accountId, bool showTransaction = false)
        {
            ILiteCollection<Account> accounts = DataContext.accounts;
            if (showTransaction)
                accounts.Include(x=> x.Transactions);
            var account = accounts.FindById(accountId);
            if (account == null)
                throw new Exception("Account not found");
            return account;
        }

        public static void ImportAccountData(Account account)
        {
            if (GetAccount(account.Id) == null)
                account = CreateAccount(account);

            List<Transaction> accountNewTransactions = account.Transactions;
            if (account.Transactions != null && account.Transactions.Any())
                TransactionModel.ImportTransactions(account.Id, accountNewTransactions);
        }

        public static void UpdateAccountTransactions(string accountId, List<Transaction> transactions)
        {
            var accounts = DataContext.accounts;
            var account = GetAccount(accountId);
            account.Transactions.AddRange(transactions);
            accounts.Update(account);
            // Index document using a document property
            accounts.EnsureIndex(x => x.Id);
        }

    }
}