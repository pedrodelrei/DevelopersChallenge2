using System;
using System.Collections.Generic;
using LiteDB;

namespace Backend.Models
{
    
    public static partial class DataContext
    {
        public static LiteDB.ILiteCollection<Account> accounts = db.GetCollection<Account>("accounts");
    }

    public class Account
    {
        [BsonId]
        public string Id { get; set; }
        public AccountTypeEnum Type { get; set; }
        [BsonIgnore]
        public List<Transaction> Transactions { get; set; }

        public Account(){}
        public Account(string bId, string aId, AccountTypeEnum t)
        {
            Id = string.Format("{0}-{1}", bId, aId);
        }
    }

    public enum AccountTypeEnum
    {
        CHECKING = 1,
        SAVINGS = 2,
        MONEYMRKT = 3,
        CREDITLINE = 4,
    }
}
