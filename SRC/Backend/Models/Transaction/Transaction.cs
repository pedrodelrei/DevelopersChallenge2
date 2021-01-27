using System;
using LiteDB;

namespace Backend.Models
{
    
    public static partial class DataContext
    {
        public static ILiteCollection<Transaction> transactions = db.GetCollection<Transaction>("transactions");
    }
    public class Transaction
    {
        public int Id { get; set; }
        [BsonIgnore]
        public TransactionTypeEnum Type { get; set; }
        public DateTime Date { get; set; }
        public float Value { get; set; }
        public string Memo { get; set; }
    }

    public enum TransactionTypeEnum
    {
        CREDIT = 1,
        DEBIT = 2,
        INT = 3,
        DIV = 4,
        FEE = 5,
        SRVCHG = 6,
        DEP = 7,
        ATM = 8,
        POS = 9,
        XFER = 10,
        CHECK = 11,
        PAYMENT = 12,
        CASH = 13,
        DIRECTDEP = 14,
        DIRECTDEBIT = 15,
        REPEATPMT = 16,
        OTHER = 17
    }
}
