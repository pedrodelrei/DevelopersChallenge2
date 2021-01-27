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
            var accounts = DataContext.accounts;
            if (accounts.FindById(newAccount.Id) != null)
                throw new Exception("Account already exists");
            accounts.Insert(newAccount);
            return newAccount;
        }

        public static Account GetAccount(string accountId, bool showTransaction = false)
        {
            ILiteCollection<Account> accounts = DataContext.accounts;
            if (showTransaction)
                accounts.Include(x=> x.Transactions);
            return accounts.FindById(accountId);
        }

        public static int ImportAccountData(Account account)
        {
            if (GetAccount(account.Id) == null)
                account = CreateAccount(account);

            List<Transaction> accountNewTransactions = account.Transactions;
            if (account.Transactions != null && account.Transactions.Any())
            {
                return TransactionModel.CreateTransactions(accountNewTransactions).Count();
            }
            return 0;
        }
    }
}