using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Backend.Models;
using Microsoft.AspNetCore.Http;

namespace Backend.Utils
{
    public static class OFXFileReader
    {

        public static Account ReadFile(IFormFile file)
        {
            #region variables
            bool readingAccountInfo = false;
            bool readingTransactions = false;
            bool transactionStart = false;

            string bankId = null;
            string acctId = null;
            string acctType = null;
            Account account = null;

            string trnType = null;
            string trnDate = null;
            string trnAmt = null;
            string memo = null;
            #endregion
            List<Transaction> transactions = null;

            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var lineContent = reader.ReadLine();
                    #region Reading Account Info
                    if (lineContent.Contains("<BANKACCTFROM>"))
                    {
                        if (readingAccountInfo)
                            throw new System.Exception();
                        readingAccountInfo = true;
                    }
                    else if (lineContent.Contains("</BANKACCTFROM>"))
                    {
                        if (!readingAccountInfo)
                            throw new System.Exception();
                        account = ParseAccount(bankId, acctId, acctType);
                        readingAccountInfo = false;
                    }
                    if (readingAccountInfo)
                    {
                        if (lineContent.Contains("<BANKID>"))
                            bankId = lineContent.Replace("<BANKID>", "");
                        if (lineContent.Contains("<ACCTID>"))
                            acctId = lineContent.Replace("<ACCTID>", "");
                        if (lineContent.Contains("<ACCTTYPE>"))
                            acctType = lineContent.Replace("<ACCTTYPE>", "");
                    }
                    #endregion
                    #region Reading Transactions
                        #region Transaction list start tag
                        if (lineContent.Contains("<BANKTRANLIST>"))
                        {
                            if (account == null || readingTransactions)
                                throw new Exception();
                            readingTransactions = true;
                            transactions = new List<Transaction>();
                        }
                        if (lineContent.Contains("</BANKTRANLIST>"))
                        {
                            if (!readingTransactions || transactionStart)
                                throw new Exception();
                                readingTransactions = false;
                        }
                        #endregion
                        #region Transaction
                        if (lineContent.Contains("<STMTTRN>"))
                        {
                            if (!readingTransactions || transactionStart)
                                throw new Exception();
                            transactionStart = true;
                        }
                        if (lineContent.Contains("</STMTTRN>"))
                        {
                            if (!readingTransactions || !transactionStart)
                                throw new Exception();
                            transactions.Add(ParseTransaction(account.Id, trnType, trnDate, trnAmt, memo));
                            trnType = trnDate = trnAmt = memo = null;
                            transactionStart = false;
                        }
                        if (readingTransactions && transactionStart)
                        {
                            if (lineContent.Contains("<TRNTYPE>"))
                            {
                                if (trnType != null)
                                    throw new Exception();
                                trnType = lineContent.Replace("<TRNTYPE>", "");
                            }
                            if (lineContent.Contains("<DTPOSTED>"))
                            {
                                if (trnDate != null)
                                    throw new Exception();
                                trnDate = lineContent.Replace("<DTPOSTED>", "");
                            }
                            if (lineContent.Contains("<TRNAMT>"))
                            {
                                if (trnAmt != null)
                                    throw new Exception();
                                trnAmt = lineContent.Replace("<TRNAMT>", "");
                            }
                            if (lineContent.Contains("<MEMO>"))
                            {
                                if (memo != null)
                                    throw new Exception();
                                memo = lineContent.Replace("<MEMO>", "");
                            }
                        }
                        #endregion
                    #endregion
                }
                
            }

            if (account == null || transactions == null)
                throw new System.Exception();
            account.Transactions = transactions;
            return account;
        }

        private static Account ParseAccount(string bankId, string acctId, string acctType)
        {
            bankId = RemoveTabsAndSpaces(bankId);
            acctId = RemoveTabsAndSpaces(acctId);
            acctType = RemoveTabsAndSpaces(acctType);

            AccountTypeEnum parsedAccountType = AccountTypeEnum.CHECKING;

            if (string.IsNullOrEmpty(bankId) || string.IsNullOrEmpty(acctId) ||
                    !Enum.TryParse(acctType, out parsedAccountType))
                throw new System.Exception();
            
            return new Account(bankId, acctId, parsedAccountType);
        }

        private static Transaction ParseTransaction(string accountId, string tType, string tDate, string tAmt, string tMemo)
        {
            tType = RemoveTabsAndSpaces(tType);
            tDate = RemoveTabsAndSpaces(tDate);
            tAmt = RemoveTabsAndSpaces(tAmt).Replace('.',',');
            tMemo = RemoveTabsAndSpaces(tMemo);

            TransactionTypeEnum parsedTransactionType = TransactionTypeEnum.ATM;
            float parsedAmount;
            DateTime parsedDate = DateTime.Now;
            parsedDate = ParseDatetime(tDate);

            if (!Enum.TryParse(tType, out parsedTransactionType) ||
                    !float.TryParse(tAmt, out parsedAmount) ||
                    string.IsNullOrEmpty(tMemo))
                throw new Exception();
                return new Transaction {
                    AccountId = accountId,
                    Type = parsedTransactionType,
                    Value = parsedAmount,
                    Date = parsedDate,
                    Memo = tMemo
                };
        }

        private static DateTime ParseDatetime(string rawDate)
        {
            if (string.IsNullOrEmpty(rawDate) || rawDate.Length < 14)
            throw new Exception();
            
            CultureInfo ptBR = new CultureInfo("pt-BR");
            var date = rawDate.Substring(0,14);
            var parsedDateTime = DateTime.Now;
            if (!DateTime.TryParseExact(date, "yyyyMMddHHmmss", ptBR, DateTimeStyles.None, out parsedDateTime))
                throw new Exception();
            return parsedDateTime;
        }
        private static string RemoveTabsAndSpaces(string str)
        {
            return str.Replace("\n","").Replace("\t","");
        }


    }
}